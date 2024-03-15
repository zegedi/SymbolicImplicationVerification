using System;

namespace SymbolicImplicationVerification.Formulas
{
    public class ImplicationFormula : BinaryOperationFormula
    {
        #region Constructors

        public ImplicationFormula(Formula leftOperand, Formula rightOperand)
            : this(null, leftOperand, rightOperand) { }

        public ImplicationFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion
    }
}
