global using BoundedIntegerType =
    SymbolicImplicationVerification.Types.BoundedInteger<
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
        SymbolicImplicationVerification.Types.IntegerType,
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
        SymbolicImplicationVerification.Types.IntegerType>;

using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;

namespace SymbolicImplicationVerification.Types
{
    public abstract class BoundedInteger<LTerm, LType, RTerm, RType> : IntegerType
        where LTerm : Term<LType>
        where LType : IntegerType
        where RTerm : Term<RType>
        where RType : IntegerType
    {
        #region Fields

        protected LTerm lowerBound;
        protected RTerm upperBound;

        #endregion

        #region Constructors

        protected BoundedInteger(LTerm lowerBound, RTerm upperBound)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Public properties

        public virtual LTerm LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        public virtual RTerm UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }

        public abstract bool IsEmpty
        {
            get;
        }

        #endregion

        #region Implicit conversions

        public static implicit operator
            BoundedIntegerType(BoundedInteger<LTerm, LType, RTerm, RType> bounded)
        {
            return bounded;
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current bounded integer.
        /// </summary>
        /// <returns>The created deep copy of the bounded integer.</returns>
        public override abstract BoundedInteger<LTerm, LType, RTerm, RType> DeepCopy();

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract BoundedInteger<LTerm, LType, RTerm, RType> Evaluated();

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return string.Format("[{0}..{1}]", lowerBound, upperBound);
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
            return obj is BoundedInteger<LTerm, LType, RTerm, RType> other && 
                   lowerBound.Equals(other.lowerBound) &&
                   upperBound.Equals(other.upperBound);
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
