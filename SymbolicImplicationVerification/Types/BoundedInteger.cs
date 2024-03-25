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

        #endregion
    }
}
