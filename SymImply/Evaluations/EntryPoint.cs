using SymImply.Terms;
using System.Reflection;

namespace SymImply.Evaluations
{
    public struct EntryPoint<T> where T : Type
    {
        #region Fields

        /// <summary>
        /// The entry of the pattern.
        /// </summary>
        private readonly Term<T> patternEntry;

        /// <summary>
        /// The parent entry.
        /// </summary>
        private object? parentEntry = null;

        /// <summary>
        /// The property of the parent entry.
        /// </summary>
        private PropertyInfo? parentProperty = null;

        /// <summary>
        /// The matched pattern terms.
        /// </summary>
        private Dictionary<int, Term<T>> matchedPatternTerms;

        #endregion

        #region Constructors

        public EntryPoint(Term<T> patternEntry, Dictionary<int, Term<T>> matchedPatternTerms)
        {
            this.patternEntry = patternEntry;
            this.matchedPatternTerms = matchedPatternTerms;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines whether it entry point has a parent or not.
        /// </summary>
        public bool HasParent
        {
            get { return parentEntry is not null && parentProperty is not null; }
        }

        /// <summary>
        /// Gets or sets the entry of the pattern.
        /// </summary>
        public Term<T> PatternEntry
        {
            get { return patternEntry; }
        }

        /// <summary>
        /// Gets or sets the parent entry.
        /// </summary>
        public object? ParentEntry
        {
            get { return parentEntry; }
            set { parentEntry = value; }
        }

        /// <summary>
        /// Gets or sets the property of the parent entry.
        /// </summary>
        public PropertyInfo? ParentProperty
        {
            get { return parentProperty; }
            set { parentProperty = value; }
        }

        /// <summary>
        /// Gets the matched pattern terms.
        /// </summary>
        public Dictionary<int, Term<T>> MatchedPatternTerms
        {
            get { return matchedPatternTerms; }
        }

        #endregion
    }
}
