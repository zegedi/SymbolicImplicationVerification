using SymbolicImplicationVerification.Terms.Patterns;

namespace SymbolicImplicationVerification.Terms
{
    public class Variable<T> : Term<T> where T : Type
    {
        #region Fields

        protected string identifier;

        #endregion

        #region Constructors

        public Variable(Variable<T> variable) : this(variable.Identifier, variable.TermType) { }

        public Variable(string identifier, T termType) : base(termType)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        public string Identifier
        {
            get { return identifier; }
            private set { identifier = value; }
        }

        #endregion

        #region Public methods

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
            return obj is not null &&
                   obj is Variable<T> other &&
                   identifier == other.identifier;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override string ToString()
        {
            return identifier;
        }

        #endregion
    }
}
