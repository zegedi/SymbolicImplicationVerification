using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Variables;

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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format(
                "\\forall {0} \\in {1} : {2}",
                quantifiedVariable.ToString(), quantifiedVariable.TermType.ToString(), statement.ToString());
        }

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (quantifiedVariable, statement.Evaluated()) switch
        {
            (_, _) => new UniversallyQuantifiedFormula<T>(this)
        };

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
            throw new NotImplementedException();
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
