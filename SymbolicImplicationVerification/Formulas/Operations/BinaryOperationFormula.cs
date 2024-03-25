using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas.Operations
{
    public abstract class BinaryOperationFormula : Formula
    {
        #region Fields

        protected Formula leftOperand;

        protected Formula rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationFormula(Formula leftOperand, Formula rightOperand)
            : this(null, leftOperand, rightOperand) { }

        public BinaryOperationFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        public Formula LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

        public Formula RightOperand
        {
            get { return rightOperand; }
            set { rightOperand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current binary operation formula.
        /// </summary>
        /// <returns>The created deep copy of the binary operation formula.</returns>
        public override abstract BinaryOperationFormula DeepCopy();

        #endregion
    }
}
