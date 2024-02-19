using System;
using SymbolicImplicationVerification.Type;


namespace SymbolicImplicationVerification.Term
{
    public class TermIndexer
    {
        #region Fields

        private Term<BoundedInteger> index;

        #endregion

        #region Constructors

        public TermIndexer(Term<BoundedInteger> index)
        {
            this.index = index;
        }

        #endregion

        #region Public properties

        public Term<BoundedInteger> Index
        {
            get { return index; }
            private set { index = value; }
        }

        #endregion
    }
}
