using System;

namespace SymbolicImplicationVerification.Term.Operation
{
    public interface IExpression<T>
    {
        public T Expand();


    }
}
