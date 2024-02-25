
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public abstract class Constant<V, T> : BaseTerm<T> where T : Type.Type
    {
        #region Fields

        protected V value;

        #endregion

        #region Constructors

        protected Constant(V value, T termType) : this(value, termType, false) { }

        protected Constant(V value, T termType, bool valueOutOfRange) : base(termType)
        {
            if (valueOutOfRange)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.value = value;
        }

        #endregion

        #region Public properties

        public V Value
        {
            get { return value; }
            set { this.value = value; }
        }

        #endregion
    }
}
