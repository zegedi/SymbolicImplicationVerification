/*
global using IntegerTypeTermIndexre = SymbolicImplicationVerification.Terms.TermIndexer<
    SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
    SymbolicImplicationVerification.Types.IntegerType,
    SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
    SymbolicImplicationVerification.Types.IntegerType>;*/

using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms
{
    /*
    public class TermIndexer<LTerm, LType, RTerm, RType>
        where LTerm : Term<LType>
        where LType : IntegerType
        where RTerm : Term<RType>
        where RType : IntegerType
    {
        #region Fields

        private Term<BoundedInteger<LTerm, LType, RTerm, RType>> index;

        #endregion

        #region Constructors

        public TermIndexer(LTerm lowerBound, RTerm upperBound) 
            : this(new BoundedInteger<LTerm, LType, RTerm, RType>(lowerBound, upperBound)) { }

        public TermIndexer(Term<BoundedInteger<LTerm, LType, RTerm, RType>> index)
        {
            this.index = index;
        }

        #endregion

        #region Public properties

        public Term<BoundedInteger<LTerm, LType, RTerm, RType>> Index
        {
            get { return index; }
            set { index = value; }
        }

        #endregion
    }
    */
}
