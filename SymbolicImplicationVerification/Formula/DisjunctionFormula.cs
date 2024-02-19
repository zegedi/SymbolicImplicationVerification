using System;

namespace SymbolicImplicationVerification.Formula
{
    public class DisjunctionFormula : BinaryOperationFormula
    {
        #region Constructors

        public DisjunctionFormula(Formula leftOperand, Formula rightOperand)
            : this(null, leftOperand, rightOperand) { }

        public DisjunctionFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion
    }
}
