global using TypeConstant = SymbolicImplicationVerification.Terms.Constants.Constant<
    object, SymbolicImplicationVerification.Types.Type>;

using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class Constant<V, T> : Term<T> //, IMatch 
        where V : notnull
        where T : Type
    {
        #region Fields

        protected V value;

        #endregion

        #region Constructors

        protected Constant(Constant<V, T> constant)
            : this(constant.value, (T) constant.termType.DeepCopy()) { }

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

        #region Implicit conversions

        public static implicit operator TypeConstant(Constant<V, T> constant)
        {
            return new TypeConstant(constant.value, constant.termType.DeepCopy());
        }

        #endregion

        #region Public properties

        public V Value
        {
            get { return value; }
            set { this.value = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current constant.
        /// </summary>
        /// <returns>The created deep copy of the constant.</returns>
        public override Constant<V, T> DeepCopy()
        {
            return new Constant<V, T>(value, (T)termType.DeepCopy());
        }

        #endregion

        #region Public methods

        public override string Hash(HashLevel level)
        {
            switch (level)
            {
                case HashLevel.NO_CONSTANTS: 
                    return string.Empty;

                default:
                    return Convert.ToString(value)!;
            }
        }

        public override string ToString()
        {
            return Convert.ToString(value)!;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is Constant<V, T> other && value.Equals(other.Value);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
