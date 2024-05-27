global using IntegerTypeVariable = SymImply.Terms.Variables.Variable<SymImply.Types.IntegerType>;

namespace SymImply.Terms.Variables
{
    public class Variable<T> : Term<T> where T : Type
    {
        #region Fields

        /// <summary>
        /// The indentifier of the variable.
        /// </summary>
        protected string identifier;

        #endregion

        #region Constructors

        public Variable(Variable<T> variable) : this(variable.identifier, (T) variable.termType.DeepCopy()) { }

        public Variable(string identifier, T termType) : base(termType)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the indentifier of the variable.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current variable.
        /// </summary>
        /// <returns>The created deep copy of the variable.</returns>
        public override Variable<T> DeepCopy()
        {
            return new Variable<T>(this);
        }

        /// <summary>
        /// Evaluated the given variable, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Variable<T> Evaluated()
        {
            return new Variable<T>(this);
        }

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            return Convert.ToString(identifier);
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
            return obj is Variable<T> other && identifier == other.identifier;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return identifier;
        }

        #endregion
    }
}
