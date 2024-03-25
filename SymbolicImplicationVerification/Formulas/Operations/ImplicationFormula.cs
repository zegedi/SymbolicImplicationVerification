using System;
using SymbolicImplicationVerification.Formulas.Operations;

namespace SymbolicImplicationVerification.Formulas
{
    public class ImplicationFormula : BinaryOperationFormula
    {
        #region Constructors

        public ImplicationFormula(ImplicationFormula implication) : base(
            implication.identifier, 
            implication.leftOperand.DeepCopy(), 
            implication.rightOperand.DeepCopy()) { }

        public ImplicationFormula(Formula leftOperand, Formula rightOperand)
            : base(null, leftOperand, rightOperand) { }

        public ImplicationFormula(string? identifier, Formula leftOperand, Formula rightOperand)
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
            (Formula left, FALSE        ) => left.Negated(),
            (TRUE        , Formula right) => right,
            (_           , TRUE         ) => TRUE.Instance(),
            (FALSE       , _            ) => TRUE.Instance(),
            (_           , _            ) => new ImplicationFormula(this)
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
            return new DisjunctionFormula(leftOperand.DeepCopy(), rightOperand.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current implication formula.
        /// </summary>
        /// <returns>The created deep copy of the implication formula.</returns>
        public override ImplicationFormula DeepCopy()
        {
            return new ImplicationFormula(this);
        }

        #endregion
    }
}
