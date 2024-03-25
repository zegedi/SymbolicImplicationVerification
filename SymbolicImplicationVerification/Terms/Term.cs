global using TypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>;
global using IntegerTypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>;
global using LogicalTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Logical>;

using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Terms
{
    public abstract class Term<T> : IMatch, IDeepCopy<Term<T>> where T : Type
    {
        #region Fields

        protected T termType;

        #endregion

        #region Constructors

        public Term(T termType)
        {
            this.termType = termType;
        }

        #endregion

        #region Public properties

        public T TermType
        {
            get { return termType; }
            private set
            {
                termType = value;
            }
        }

        #endregion

        #region Public abstract methods

        public abstract string Hash(HashLevel level);

        /// <summary>
        /// Create a deep copy of the current term.
        /// </summary>
        /// <returns>The created deep copy of the term.</returns>
        public abstract Term<T> DeepCopy();

        #endregion

        #region Public virtual methods

        public virtual bool Matches(object? obj)
        {
            return Equals(obj);
        }

        #endregion

        #region Implicit conversions 


        public static implicit operator TypeTerm(Term<T> term)
        {
            return term;
        }

        #endregion
    }
}
