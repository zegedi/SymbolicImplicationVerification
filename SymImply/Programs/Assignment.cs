using SymImply.Evaluations;
using SymImply.Formulas;
using SymImply.Types;
using SymImply.Terms.Variables;
using SymImply.Terms;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations.Binary;
using SymImply.Terms.Operations;
using System.Text;
using SymImply.Formulas.Operations;

namespace SymImply.Programs
{
    public class Assignment : Program
    {
        #region Fields

        /// <summary>
        /// The list of integer assignments.
        /// </summary>
        private List<(Variable<IntegerType>, IntegerTypeTerm)> integerAssignments;

        /// <summary>
        /// The list of logical assignments.
        /// </summary>
        private List<(Variable<Logical>, LogicalTerm)> logicalAssignments;

        #endregion

        #region Constructors

        public Assignment(Assignment assingmentProgram)
        {

            integerAssignments =
                new List<(Variable<IntegerType>, IntegerTypeTerm)>(assingmentProgram.integerAssignments.Count);

            logicalAssignments =
                new List<(Variable<Logical>, LogicalTerm)>(assingmentProgram.logicalAssignments.Count);

            foreach ((Variable<Logical> var, LogicalTerm value) assign in assingmentProgram.logicalAssignments)
            {
                logicalAssignments.Add((assign.var.DeepCopy(), assign.value.DeepCopy()));
            }

            foreach ((Variable<IntegerType> var, IntegerTypeTerm value) assign in assingmentProgram.integerAssignments)
            {
                integerAssignments.Add((assign.var.DeepCopy(), assign.value.DeepCopy()));
            }
        }

        public Assignment(List<(Variable<IntegerType>, IntegerTypeTerm)> integerAssignments)
            : this(integerAssignments, new List<(Variable<Logical>, LogicalTerm)>()) { }

        public Assignment(List<(Variable<Logical>, LogicalTerm)> logicalAssignments)
            : this(new List<(Variable<IntegerType>, IntegerTypeTerm)>(), logicalAssignments) { }

        public Assignment(
            List<(Variable<IntegerType>, IntegerTypeTerm)> integerAssignments,
            List<(Variable<Logical>, LogicalTerm)> logicalAssignments)
        {
            this.integerAssignments = integerAssignments;
            this.logicalAssignments = logicalAssignments;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the list of logical assignments.
        /// </summary>
        public List<(Variable<IntegerType>, IntegerTypeTerm)> IntegerAssingments
        {
            get { return integerAssignments; }
            set { integerAssignments = value; }
        }

        /// <summary>
        /// Gets or sets the list of logical assignments.
        /// </summary>
        public List<(Variable<Logical>, LogicalTerm)> LogicalAssignments
        {
            get { return logicalAssignments; }
            set { logicalAssignments = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder variableStringBuilder = new StringBuilder();
            StringBuilder valueStringBuilder    = new StringBuilder();

            string delimiter = string.Empty;

            foreach ((Variable<IntegerType> variable, IntegerTypeTerm value) assign in integerAssignments)
            {
                variableStringBuilder.Append(delimiter);
                variableStringBuilder.Append(assign.variable.ToString());

                valueStringBuilder.Append(delimiter);
                valueStringBuilder.Append(assign.value.ToString());

                delimiter = ", ";
            }

            foreach ((Variable<Logical> variable, LogicalTerm value) assign in logicalAssignments)
            {
                variableStringBuilder.Append(delimiter);
                variableStringBuilder.Append(assign.variable.ToString());

                valueStringBuilder.Append(delimiter);
                valueStringBuilder.Append(assign.value.ToString());

                delimiter = ", ";
            }

            return string.Format(
                "\\assign{{{0}}}{{{1}}}", variableStringBuilder.ToString(), valueStringBuilder.ToString());
        }

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
                equal = integerAssignments.Count == other.integerAssignments.Count &&
                        logicalAssignments.Count == other.logicalAssignments.Count;

                for (int index = 0; equal && index < integerAssignments.Count; ++index)
                {
                    (Variable<IntegerType> var, IntegerTypeTerm val) thisAssignment  = integerAssignments[index];
                    (Variable<IntegerType> var, IntegerTypeTerm val) otherAssignment = other.integerAssignments[index];

                    equal &= thisAssignment.var.Equals(otherAssignment.var) &&
                             thisAssignment.val.Equals(otherAssignment.val);
                }

                for (int index = 0; equal && index < logicalAssignments.Count; ++index)
                {
                    (Variable<Logical> var, LogicalTerm val) thisAssignment  = logicalAssignments[index];
                    (Variable<Logical> var, LogicalTerm val) otherAssignment = other.logicalAssignments[index];

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

        /// <summary>
        /// Substitute the assignments into the given formula.
        /// </summary>
        /// <param name="formula">The formula to substitute into.</param>
        /// <returns>The result of the substitution.</returns>
        public Formula SubstituteAssignments(Formula formula)
        {
            if (formula is FALSE or NotEvaluable)
            {
                return FALSE.Instance();
            }

            LinkedList<Formula> linearOperands;

            if (formula is BinaryOperationFormula binaryOperation)
            {
                linearOperands = binaryOperation.LinearOperands();

                if (linearOperands.Count == 1)
                {
                    formula = linearOperands.First();
                    linearOperands.RemoveFirst();
                }
                else
                {
                    LinkedList<Formula> binarize = new LinkedList<Formula>();

                    foreach (Formula lin in linearOperands)
                    {
                        binarize.AddLast(lin);
                    }

                    formula = binaryOperation.Binarize(binarize);
                }
            }
            else
            {
                linearOperands = new LinkedList<Formula>();
            }

            StringBuilder replaceString = new StringBuilder();

            List<(IntegerTypeTerm value, LinkedList<EntryPoint<IntegerType>> entryPoints)> integerEntries
                = new List<(IntegerTypeTerm, LinkedList<EntryPoint<IntegerType>>)>(integerAssignments.Count);

            List<(LogicalTerm value, LinkedList<EntryPoint<Logical>> entryPoints)> logicalEntries
                = new List<(LogicalTerm, LinkedList<EntryPoint<Logical>>)>(integerAssignments.Count);

            LinkedList<Formula> constraints = new LinkedList<Formula>();
            LinkedList<Formula> changeIdent = new LinkedList<Formula>();

            foreach ((Variable<IntegerType> var, IntegerTypeTerm value) assign in integerAssignments)
            {
                if (replaceString.Length > 0)
                {
                    replaceString.Append(", ");
                }

                replaceString.AppendFormat("{0} \\leftarrow {1}", assign.var, assign.value);

                Variable<IntegerType> variable = assign.var;
                IntegerTypeTerm value = assign.value.Evaluated();

                IntegerType variableType = variable.TermType;
                IntegerType valueType    = value.TermType;

                bool constantValueOutOfRange = 
                    value is IntegerTypeConstant constant && variableType.IsValueOutOfRange(constant.Value);

                if (constantValueOutOfRange)
                {
                    return new WeakestPrecondition(ABORT.Instance(), formula);
                }

                if (formula is not TRUE)
                {
                    integerEntries.Add(
                        (assign.value, PatternReplacer<IntegerType>.FindEntryPoints(formula, variable)));
                }

                foreach (Formula linearOperand in linearOperands)
                {
                    if (linearOperand.HasIdentifier && !changeIdent.Contains(linearOperand))
                    {
                        var entry = PatternReplacer<IntegerType>.FindEntryPoints(linearOperand, variable);

                        if (entry.Count > 0)
                        {
                            changeIdent.AddLast(linearOperand);
                        }
                    }
                }

                if (!variableType.TypeAssignable(valueType) && value is not IntegerTypeConstant)
                {
                    constraints.AddLast(variableType.TypeConstraintOn(assign.value));
                }
            }

            foreach ((Variable<Logical> var, LogicalTerm value) assign in logicalAssignments)
            {
                if (replaceString.Length > 0)
                {
                    replaceString.Append(", ");
                }
                replaceString.AppendFormat("{0} \\leftarrow {1}", assign.var, assign.value);

                if (formula is not TRUE)
                {
                    logicalEntries.Add((assign.value, PatternReplacer<Logical>.FindEntryPoints(formula, assign.var)));
                }

                foreach (Formula linearOperand in linearOperands)
                {
                    if (linearOperand.HasIdentifier && !changeIdent.Contains(linearOperand))
                    {
                        var entry = PatternReplacer<Logical>.FindEntryPoints(linearOperand, assign.var);

                        if (entry.Count > 0)
                        {
                            changeIdent.AddLast(linearOperand);
                        }
                    }
                }
            }

            formula.Identifier = string.Format(
                formula.HasIdentifier ? "{0}^{{{1}}}" : "({0})^{{{1}}}", formula, replaceString.ToString());

            foreach (Formula changeIndetifier in changeIdent)
            {
                changeIndetifier.Identifier = string.Format(
                    "{0}^{{{1}}}", changeIndetifier, replaceString.ToString());
            }

            foreach ((IntegerTypeTerm value, LinkedList<EntryPoint<IntegerType>> entryPoints) info in integerEntries)
            {
                PatternReplacer<IntegerType>.VariableReplaced(info.entryPoints, info.value);
            }

            foreach ((LogicalTerm value, LinkedList<EntryPoint<Logical>> entryPoints) info in logicalEntries)
            {
                PatternReplacer<Logical>.VariableReplaced(info.entryPoints, info.value);
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
