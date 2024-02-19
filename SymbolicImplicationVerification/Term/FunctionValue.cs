using System;

namespace SymbolicImplicationVerification.Term
{
    public abstract class FunctionValue<D,T> : Term<T> 
        where D : Type.Type
        where T : Type.Type
    {
        #region Fields

        protected Term<D> argument;

        #endregion

        #region Constructors

        public FunctionValue(Term<D> argument, T termTpye) : base(termTpye)
        {
            this.argument = argument;
        }

        #endregion

        #region Public properties

        public Term<D> Argument
        {
            get { return argument; }
            private set { argument = value; }
        }

        #endregion
    }
}
