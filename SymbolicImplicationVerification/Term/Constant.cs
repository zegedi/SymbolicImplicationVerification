
// Aliases for the different types.
global using LogicalConstant = SymbolicImplicationVerification.Term.Constant<bool, SymbolicImplicationVerification.Type.Logical>;
global using IntegerConstant = SymbolicImplicationVerification.Term.Constant<int, SymbolicImplicationVerification.Type.Integer>;
global using NaturalNumberConstant = SymbolicImplicationVerification.Term.Constant<int, SymbolicImplicationVerification.Type.NaturalNumber>;
global using PositiveIntegerConstant = SymbolicImplicationVerification.Term.Constant<int, SymbolicImplicationVerification.Type.PositiveInteger>;
global using ZeroOrOneConstant = SymbolicImplicationVerification.Term.Constant<int, SymbolicImplicationVerification.Type.ZeroOrOne>;
global using BoundedIntegerConstant = SymbolicImplicationVerification.Term.Constant<int, SymbolicImplicationVerification.Type.BoundedInteger>;

namespace SymbolicImplicationVerification.Term
{
    public class Constant<V, T> : Term<T> where T : Type.Type
    {
        #region Fields

        private V value;

        #endregion

        #region Constructors

        public Constant(V value, T typeType) : base(typeType)
        {
            this.value = value;
        }

        #endregion

        #region Public properties

        public V Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        #endregion
    }
}
