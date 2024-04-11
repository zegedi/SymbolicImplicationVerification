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

        public IntegerTypeAnythingPattern(IntegerTypeAnythingPattern anythingPattern)
            : base(anythingPattern.identifier, anythingPattern.termType.DeepCopy()) { }

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

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current anything pattern.
        /// </summary>
        /// <returns>The created deep copy of the anything pattern.</returns>
        public override IntegerTypeAnythingPattern DeepCopy()
        {
            return new IntegerTypeAnythingPattern(this);
        }

        /// <summary>
        /// Evaluated the given pattern, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override IntegerTypeAnythingPattern Evaluated()
        {
            return new IntegerTypeAnythingPattern(this);
        }

        #endregion
    }
}
