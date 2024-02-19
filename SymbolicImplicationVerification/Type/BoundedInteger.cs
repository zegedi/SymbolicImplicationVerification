using System;
using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Type
{
    public class BoundedInteger : IntegerType
    {
        #region Fields

        private Term<Integer> lowerBound;
        private Term<Integer> upperBound;

        #endregion

        #region Constructors

        // TODO : copy constructor
        public BoundedInteger(Term<Integer> lowerBound, Term<Integer> upperBound)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Public properties

        public Term<Integer> LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        public Term<Integer> UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }
 
        #endregion
    }
}
