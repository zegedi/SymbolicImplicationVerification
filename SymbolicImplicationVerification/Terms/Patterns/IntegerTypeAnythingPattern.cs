using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class IntegerTypeAnythingPattern : AnythingPattern<IntegerType>
    {
        #region Constructors

        public IntegerTypeAnythingPattern(int identifier) : this(identifier, Integer.Instance()) { }

        public IntegerTypeAnythingPattern(int identifier, IntegerType termType) : base(identifier, termType) { }

        #endregion

        #region Public static operators

        public static Addition operator +(IntegerTypeAnythingPattern pattern, IntegerTypeTerm term)
        {
            return new Addition(pattern, term);
        }

        public static Subtraction operator -(IntegerTypeAnythingPattern pattern, IntegerTypeTerm term)
        {
            return new Subtraction(pattern, term);
        }

        public static Multiplication operator *(IntegerTypeAnythingPattern pattern, IntegerTypeTerm term)
        {
            return new Multiplication(pattern, term);
        }

        #endregion
    }
}
