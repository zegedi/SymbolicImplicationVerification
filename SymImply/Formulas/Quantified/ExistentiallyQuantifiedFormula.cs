using SymImply.Evaluations;
using SymImply.Formulas.Relations;
using SymImply.Terms;
using SymImply.Terms.Variables;
using SymImply.Types;

namespace SymImply.Formulas.Quantified
{
    public class ExistentiallyQuantifiedFormula<T> : QuantifiedFormula<T> where T : Type
    {
        #region Constructors

        public ExistentiallyQuantifiedFormula(ExistentiallyQuantifiedFormula<T> quantified) : base(
            quantified.identifier,
            quantified.quantifiedVariable.DeepCopy(),
            quantified.statement.DeepCopy()) { }

        public ExistentiallyQuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) :
            base(null, quantifiedVariable, statement) { }

        public ExistentiallyQuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement)
            : base(identifier, quantifiedVariable, statement) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            bool variableFromEmptyset = false;

            if (quantifiedVariable.TermType is BoundedIntegerType bounded)
            {
                variableFromEmptyset = bounded.IsEmpty && bounded.Evaluated() == bounded;
            }

            return string.Format(
                "\\existentially{{{0}}}{{{1}}}{{{2}}}", quantifiedVariable, 
                variableFromEmptyset ? "\\emptyset" : quantifiedVariable.TermType, statement
            );
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            if (statement is FALSE or TRUE or NotEvaluable)
            {
                return statement.DeepCopy();
            }

            ExistentiallyQuantifiedFormula<T> result = DeepCopy();

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

            if (quantifiedVariable  is IntegerTypeVariable quantified && 
                quantified.TermType is BoundedIntegerType  bounded)
            {
                if (bounded.IsEmpty)
                {
                    return FALSE.Instance();
                }

                if (bounded.LowerBound.Equals(bounded.UpperBound))
                {
                    return PatternReplacer<IntegerType>.VariableReplaced(
                        statement, quantified.DeepCopy(), bounded.LowerBound);
                }
            }

            return ReturnOrDeepCopy(
                new ExistentiallyQuantifiedFormula<T>(quantifiedVariable.DeepCopy(), statement.Evaluated())
            );
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
            return obj is ExistentiallyQuantifiedFormula<T> other &&
                   quantifiedVariable.Equals(other.quantifiedVariable) &&
                   quantifiedVariable.TermType.Equals(other.quantifiedVariable.TermType) &&
                   statement.Equals(other.statement);
        }

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public override Formula ConjunctionWith(Formula other)
        {
            if (other is ExistentiallyQuantifiedFormula<T> existentiallyQuantified)
            {
                T? intersection = (T?) quantifiedVariable.TermType.Intersection(
                    existentiallyQuantified.quantifiedVariable.TermType);

                if (intersection is not null && StatementsEquivalent(existentiallyQuantified))
                {
                    ExistentiallyQuantifiedFormula<T> result = DeepCopy();

                    result.quantifiedVariable.TermType = intersection;
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

            if (this is ExistentiallyQuantifiedFormula<IntegerType> quantified)
            {
                return ConjunctionWith(quantified.DeepCopy(), other.DeepCopy());
            }

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
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
        /// Determines whether the specified program is equivalent to the current program.
        /// </summary>
        /// <param name="other">The program to compare with the current program.</param>
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
            return new UniversallyQuantifiedFormula<T>(quantifiedVariable.DeepCopy(), statement.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current existentially quantified program.
        /// </summary>
        /// <returns>The created deep copy of the existentially quantified program.</returns>
        public override ExistentiallyQuantifiedFormula<T> DeepCopy()
        {
            return new ExistentiallyQuantifiedFormula<T>(this);
        }

        #endregion
    }
}
