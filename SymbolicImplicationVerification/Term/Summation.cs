using System;
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class Summation<T> : Term<T> where T : IntegerType
    {
        #region Fields

        protected TermIndexer index;

        protected Term<T> argument;

        #endregion

        #region Constructors

        public Summation(T termType, Term<T> argument, Term<BoundedInteger> bounds) : base(termType)
        {
            index = new TermIndexer(bounds);

            this.argument = argument;
        }

        #endregion

        #region Public properties

        public Term<T> Argument
        {
            get { return argument; }
            private set { argument = value; }
        }

        #endregion
    }
}
