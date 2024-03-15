using System;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class UnaryOperationFormula : Formula
    {
        #region Fields

        protected Formula operand;

        #endregion

        #region Constructors

        public UnaryOperationFormula(Formula operand) : this(null, operand) { }

        public UnaryOperationFormula(string? identifier, Formula operand) : base(identifier)
        {
            this.operand = operand;
        }

        #endregion

        #region Public properties

        public Formula Operand
        {
            get { return operand; }
            set { operand = value; }
        }

        #endregion
    }
}
