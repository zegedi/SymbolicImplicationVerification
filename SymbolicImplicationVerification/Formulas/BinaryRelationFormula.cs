using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class BinaryRelationFormula<T> : Formula where T : Types.Type
    {
        #region Fields

        protected Term<T> leftComponent;

        protected Term<T> rightComponent;

        #endregion

        #region Constructors

        public BinaryRelationFormula(Term<T> leftComponent, Term<T> rightComponent) : this(null, leftComponent, rightComponent) { }

        public BinaryRelationFormula(string? identifier, Term<T> leftComponent, Term<T> rightComponent) : base(identifier)
        {
            this.leftComponent  = leftComponent;
            this.rightComponent = rightComponent;
        }

        #endregion

        #region Public properties

        public Term<T> LeftComponent
        {
            get { return leftComponent; }
            set { leftComponent = value; }
        }

        public Term<T> RightComponent
        {
            get { return rightComponent; }
            set { rightComponent = value; }
        }

        #endregion
    }
}
