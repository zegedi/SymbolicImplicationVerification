global using IntegerTypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>;
global using LogicalTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Logical>;
global using TypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>;

namespace SymbolicImplicationVerification.Terms
{
    public abstract class Term<T> : IMatch, IEvaluable<Term<T>>, IDeepCopy<Term<T>> where T : Type
    {
        #region Fields

        /// <summary>
        /// The type of the term.
        /// </summary>
        protected T termType;

        #endregion

        #region Constructors

        public Term(T termType)
        {
            this.termType = termType;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the term type.
        /// </summary>
        public T TermType
        {
            get { return termType; }
            set { termType = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public abstract string Hash(HashLevel level);

        /// <summary>
        /// Create a deep copy of the current term.
        /// </summary>
        /// <returns>The created deep copy of the term.</returns>
        public abstract Term<T> DeepCopy();

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract Term<T> Evaluated();

        #endregion

        #region Public virtual methods

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public virtual bool Matches(object? obj)
        {
            return Equals(obj);
        }

        #endregion
    }
}
