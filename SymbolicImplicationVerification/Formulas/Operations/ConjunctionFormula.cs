using System;
using SymbolicImplicationVerification.Formulas.Operations;

namespace SymbolicImplicationVerification.Formulas
{
    public class ConjunctionFormula : BinaryOperationFormula
    {
        #region Constructors

        public ConjunctionFormula(ConjunctionFormula conjunction) : base(
            conjunction.identifier, 
            conjunction.leftOperand.DeepCopy(), 
            conjunction.rightOperand.DeepCopy()) { }

        public ConjunctionFormula(Formula leftOperand, Formula rightOperand)
            : base(null, leftOperand, rightOperand) { }

        public ConjunctionFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftOperand.Evaluated(), rightOperand.Evaluated()) switch
        {
            (NotEvaluable, _            ) => NotEvaluable.Instance(),
            (_           , NotEvaluable ) => NotEvaluable.Instance(),
            (Formula left, TRUE         ) => left,
            (TRUE        , Formula right) => right,
            (_           , FALSE        ) => FALSE.Instance(),
            (FALSE       , _            ) => FALSE.Instance(),
            (_           , _            ) => new ConjunctionFormula(this)
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
            return new DisjunctionFormula(leftOperand.Negated(), rightOperand.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current conjunction formula.
        /// </summary>
        /// <returns>The created deep copy of the conjunction formula.</returns>
        public override ConjunctionFormula DeepCopy()
        {
            return new ConjunctionFormula(this);
        }

        #endregion
    }
}
