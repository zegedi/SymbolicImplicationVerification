using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public abstract class IntConstant<T> : Constant<int, T> 
        where T : IntegerType
    {
        #region Constructors

        public IntConstant(int value, T type) : base(value, type) { }

        #endregion
    }
}
