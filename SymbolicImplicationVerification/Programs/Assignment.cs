
// Aliases for the variable and assigned value typeClasses.
global using AssignedValue = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>;
global using Variable = SymbolicImplicationVerification.Terms.Variables.Variable<SymbolicImplicationVerification.Types.Type>;

using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Programs
{
    public class Assignment : Program
    {
        #region Fields

        private List<(Variable, AssignedValue)> assingments;

        #endregion

        #region Constructors

        public Assignment(Assignment assingmentProgram)
        {
            assingments = new List<(Variable, AssignedValue)>(assingmentProgram.assingments.Count);

            foreach ((Variable var, AssignedValue value) assign in assingmentProgram.assingments)
            {
                assingments.Add((assign.var.DeepCopy(), assign.value.DeepCopy()));
            }
        }

        public Assignment(List<(Variable, AssignedValue)> assingments)
        {
            this.assingments = assingments;
        }

        #endregion

        #region Public properties

        public List<(Variable, AssignedValue)> Assingments
        {
            get { return assingments; }
            set { assingments = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the current program.
        /// </summary>
        /// <returns>The created deep copy of the program.</returns>
        public override Assignment DeepCopy()
        {
            return new Assignment(this);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            bool equal = false;

            if (obj is Assignment other)
            {
                equal = assingments.Count == other.assingments.Count;

                for (int index = 0; equal && index < assingments.Count; ++index)
                {
                    (Variable var, AssignedValue val) thisAssignment  = assingments[index];
                    (Variable var, AssignedValue val) otherAssignment = other.assingments[index];

                    equal &= thisAssignment.var.Equals(otherAssignment.var) &&
                             thisAssignment.val.Equals(otherAssignment.val);
                }
            }

            return equal;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Formula SubstituteAssignments(Formula formula)
        {
            if (formula is FALSE)
            {
                return FALSE.Instance();
            }

            List<(AssignedValue value, LinkedList<EntryPoint<Type>> entryPoints)> entries
                = new List<(AssignedValue, LinkedList<EntryPoint<Type>>)>(assingments.Count);

            LinkedList<Formula> constraints = new LinkedList<Formula>();

            foreach ((Variable var, AssignedValue value) assign in assingments)
            {
                Variable variable   = assign.var;
                AssignedValue value = assign.value;

                Type variableType = variable.TermType;
                Type valueType    = value.TermType;

                if (!variableType.TypeCompatible(valueType))
                {
                    return new WeakestPrecondition(ABORT.Instance(), formula);
                }

                if (value is TypeBinaryOperationTerm operation)
                {
                    value = operation.Simplified();
                    valueType = value.TermType;
                }

                if (value is TypeConstant constant && variableType.IsValueOutOfRange(constant.Value))
                {
                    return new WeakestPrecondition(ABORT.Instance(), formula);
                }

                if (formula is not TRUE)
                {
                    entries.Add((assign.value, PatternReplacer<Type>.FindEntryPoints(formula, variable)));
                }

                // If the assignment is partial.
                if (!variableType.TypeAssignable(valueType))
                {
                    constraints.AddLast(variableType.TypeConstraintOn(assign.value));
                }
            }

            foreach ((AssignedValue value, LinkedList<EntryPoint<Type>> entryPoints) info in entries)
            {
                PatternReplacer<Type>.VariableReplaced(info.entryPoints, info.value);
            }

            foreach (Formula constraint in constraints)
            {
                formula = formula is TRUE ? constraint : new ConjunctionFormula(formula, constraint);
            }

            return formula;
        }

        #endregion
    }
}
