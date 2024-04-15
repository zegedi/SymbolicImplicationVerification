using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class UniversallyQuantifiedFormula<T> : QuantifiedFormula<T> where T : Types.Type
    {
        #region Constructors

        public UniversallyQuantifiedFormula(UniversallyQuantifiedFormula<T> quantified) : base(
            quantified.identifier,
            quantified.quantifiedVariable.DeepCopy(),
            quantified.statement.DeepCopy()) { }

        public UniversallyQuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) :
            base(null, quantifiedVariable, statement) { }

        public UniversallyQuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement)
            : base(identifier, quantifiedVariable, statement) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            bool variableFromEmpyset = false;

            if (quantifiedVariable.TermType is BoundedIntegerType bounded)
            {
                variableFromEmpyset = bounded.IsEmpty && bounded.Evaluated() == bounded;
            }

            return string.Format(
                "\\forall {0} \\in {1} \\,\\colon {2}",
                quantifiedVariable, variableFromEmpyset ? "\\emptyset" : quantifiedVariable.TermType, statement
            );
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            UniversallyQuantifiedFormula<T> result = DeepCopy();

            if (result.quantifiedVariable.TermType is TermBoundedInteger boundedInteger)
            {
                TermBoundedInteger boundsEval = boundedInteger.Evaluated();

                if (boundsEval != boundedInteger)
                {
                    boundedInteger.LowerBound = boundsEval.LowerBound;
                    boundedInteger.UpperBound = boundsEval.UpperBound;

                    return result;
                }
            }

            return (quantifiedVariable.TermType, statement) switch
            {
                (BoundedIntegerType { IsEmpty: true }, _) => TRUE.Instance(),

                (_, FALSE       ) => FALSE.Instance(),
                (_, TRUE        ) => TRUE .Instance(),
                (_, NotEvaluable) => NotEvaluable.Instance(),
                (_, _           ) => ReturnOrDeepCopy(
                    new UniversallyQuantifiedFormula<T>(quantifiedVariable.DeepCopy(), statement.Evaluated()))
            };
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
            return obj is UniversallyQuantifiedFormula<T> other &&
                   quantifiedVariable.Equals(other.quantifiedVariable) &&
                   quantifiedVariable.TermType.Equals(other.quantifiedVariable.TermType) &&
                   statement.Equals(other.statement);
        }

        public override Formula ConjunctionWith(Formula other)
        {
            if (other is UniversallyQuantifiedFormula<T> universallyQuantified)
            {
                T? union = 
                    (T?) quantifiedVariable.TermType.Union(universallyQuantified.quantifiedVariable.TermType);

                if (union is not null && StatementsEquivalent(universallyQuantified))
                {
                    UniversallyQuantifiedFormula<T> result = DeepCopy();

                    result.quantifiedVariable.TermType = union;

                    return result;
                }
            }

            if (other is Equal<T> equal)
            {
                Formula? substituted = equal.SubstituteVariable(this);

                if (substituted is not null)
                {
                    return new ConjunctionFormula(substituted, other.DeepCopy());
                }
            }

            if (this is UniversallyQuantifiedFormula<IntegerType> quantified)
            {
                return ConjunctionWith(quantified.DeepCopy(), other.DeepCopy());
            }

            //Term<T>? variableReplaceTerm = PatternReplacer<T>.QuantifiedVariableReplaced(this, other);

            //if (variableReplaceTerm is not null && 
            //    variableReplaceTerm is IntegerTypeTerm replace &&
            //    quantifiedVariable.TermType is BoundedIntegerType bounded)
            //{
            //    IntegerConstant one = new IntegerConstant(1);

            //    Formula decreaseLowerBound = 
            //        new IntegerTypeEqual(one + replace.DeepCopy(), bounded.LowerBound.DeepCopy()).Evaluated();

            //    Formula increaseUpperBound =
            //        new IntegerTypeEqual(replace.DeepCopy(), one + bounded.UpperBound.DeepCopy()).Evaluated();

            //    if (decreaseLowerBound is TRUE || increaseUpperBound is TRUE)
            //    {
            //        bool decrease = decreaseLowerBound is TRUE;

            //        TermBoundedInteger newBounds = new TermBoundedInteger(
            //            decrease ? replace.DeepCopy() : bounded.LowerBound.DeepCopy(),
            //            decrease ? bounded.UpperBound.DeepCopy() : replace.DeepCopy()
            //        );

            //        return new UniversallyQuantifiedFormula<IntegerType>(
            //            new Variable<IntegerType>(quantifiedVariable.Identifier, newBounds),
            //            statement.DeepCopy()
            //        );
            //    }
            //}

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        public virtual Formula DisjunctionWith(BinaryRelationFormula<T> other)
        {
            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
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
        /// Determines whether the specified formula is equivalent to the current formula.
        /// </summary>
        /// <param name="other">The formula to compare with the current formula.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            return Evaluated().Equals(other.Evaluated());
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new ExistentiallyQuantifiedFormula<T>(quantifiedVariable.DeepCopy(), statement.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current universally quantified formula.
        /// </summary>
        /// <returns>The created deep copy of the universally quantified formula.</returns>
        public override UniversallyQuantifiedFormula<T> DeepCopy()
        {
            return new UniversallyQuantifiedFormula<T>(this);
        }

        #endregion
    }
}
