using SymbolicImplicationVerification.Term.Constant;
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class ArrayVariable<TermType> : Variable<TermType>
        where TermType : Type.Type
    {
        #region Fields

        protected TermIndexer index;

        #endregion

        #region Constructors

        public ArrayVariable(string identifier, Term<Integer> length, TermType termType) : base(identifier, termType)
        {
            const int firstElementIndex = 1;

            IntegerConstant firstIndex = new IntegerConstant(firstElementIndex);

            BoundedInteger arrayIndexBounds = new BoundedInteger(firstIndex, length);
        }

        #endregion

        #region Public properties

        public Term<BoundedInteger> Length
        {
            get { return index.Index; }
        }

        #endregion
    }
}
