global using IntegerTpyeTerm = SymbolicImplicationVerification.Term.Term<SymbolicImplicationVerification.Type.IntegerType>;

namespace SymbolicImplicationVerification.Term
{
    public abstract class Term<T> where T : Type.Type
    {
        #region Fields

        protected T termType;

        #endregion

        #region Constructors

        public Term(T termType)
        {
            this.termType = termType;
        }

        #endregion

        #region Public properties

        public T TermType
        { 
            get { return termType; }
            private set
            {
                termType = value;
            }
        }

        #endregion
    }
}
