using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Type
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
    }
}
