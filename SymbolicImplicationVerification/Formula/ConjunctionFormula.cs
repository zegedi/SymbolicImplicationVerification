using System;

namespace SymbolicImplicationVerification.Formula
{
    public class ConjunctionFormula : BinaryOperationFormula
    {
        #region Constructors

        public ConjunctionFormula(Formula leftOperand, Formula rightOperand)
            : this(null, leftOperand, rightOperand) { }

        public ConjunctionFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion
    }
}
