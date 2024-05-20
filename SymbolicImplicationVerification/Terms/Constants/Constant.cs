global using TypeConstant = SymbolicImplicationVerification.Terms.Constants.Constant<
    object, SymbolicImplicationVerification.Types.Type>;

using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public abstract class Constant<V, T> : Term<T> //, IMatch 
        where V : notnull
        where T : Type
    {
        #region Fields

        /// <summary>
        /// The value of the constant.
        /// </summary>
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

        #region Public properties

        /// <summary>
        /// Gets or sets the value of the constant.
        /// </summary>
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
        public override abstract Constant<V, T> DeepCopy();

        #endregion

        #region Public methods

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            switch (level)
            {
                case HashLevel.NO_CONSTANTS: 
                    return string.Empty;

                default:
                    return Convert.ToString(value) ?? string.Empty;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return Convert.ToString(value);
        }

        /// <summary>
        /// Evaluated the given constant, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Constant<V,T> Evaluated()
        {
            return DeepCopy();
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
