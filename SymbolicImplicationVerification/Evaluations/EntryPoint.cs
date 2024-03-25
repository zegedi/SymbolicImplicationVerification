using SymbolicImplicationVerification.Terms;
using System.Reflection;

namespace SymbolicImplicationVerification.Evaluations
{
    public struct EntryPoint<T> where T : Type
    {
        #region Fields

        private readonly Term<T> patternEntry;

        private object? parentEntry;

        private PropertyInfo? parentProperty;

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

        public bool HasParent
        {
            get { return parentEntry is not null && parentProperty is not null; }
        }

        public Term<T> PatternEntry
        {
            get { return patternEntry; }
        }

        public object? ParentEntry
        {
            get { return parentEntry; }
            set { parentEntry = value; }
        }

        public PropertyInfo? ParentProperty
        {
            get { return parentProperty; }
            set { parentProperty = value; }
        }

        public Dictionary<int, Term<T>> MatchedPatternTerms
        {
            get { return matchedPatternTerms; }
        }

        #endregion
    }
}
