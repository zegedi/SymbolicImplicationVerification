using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Formulas.Quantified
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
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (quantifiedVariable, statement.Evaluated()) switch
        {
            (_, _) => new ExistentiallyQuantifiedFormula<T>(this)
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
            return new UniversallyQuantifiedFormula<T>(quantifiedVariable.DeepCopy(), statement.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current existentially quantified formula.
        /// </summary>
        /// <returns>The created deep copy of the existentially quantified formula.</returns>
        public override ExistentiallyQuantifiedFormula<T> DeepCopy()
        {
            return new ExistentiallyQuantifiedFormula<T>(this);
        }

        #endregion
    }
}
