using SymbolicImplicationVerification.Types;

using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Terms
{
    public class TermIndexer
    {
        #region Fields

        private Term<BoundedIntegerType> index;

        #endregion

        #region Constructors

        public TermIndexer(Term<BoundedIntegerType> index)
        {
            this.index = index;
        }

        #endregion

        #region Public properties

        public Term<BoundedIntegerType> Index
        {
            get { return index; }
            set { index = value; }
        }

        #endregion
    }
}
