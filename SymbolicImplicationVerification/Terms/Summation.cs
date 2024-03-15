using System;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms
{
    public class Summation<T> : Term<T> where T : IntegerType
    {
        #region Fields

        //protected TermIndexer index;

        protected Term<T> argument;

        #endregion

        #region Constructors

        public Summation(T termType) : base(termType) { }

        /*
        public Summation(T termType, Term<T> argument, Term<BoundedInteger> bounds) : base(termType)
        {
            index = new TermIndexer(bounds);

            this.argument = argument;
        }
        */
        #endregion

        #region Public properties

        public Term<T> Argument
        {
            get { return argument; }
            private set { argument = value; }
        }

        #endregion

        #region Public methods

        public override string Hash(HashLevel level)
        {
            return ToString();
        }

        public override string ToString()
        {
            return "\\sum_{k=1}^n";
        }

        #endregion
    }
}
