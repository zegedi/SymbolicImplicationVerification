using System;

namespace SymbolicImplicationVerification.Term
{
    public abstract class BaseTerm<T> : Term<T> where T : Type.Type
    {
        #region Constructors

        public BaseTerm(T termType) : base(termType) { }

        #endregion
    }
}
