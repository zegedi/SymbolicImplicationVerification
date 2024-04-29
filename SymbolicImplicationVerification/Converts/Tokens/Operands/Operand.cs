using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Programs;

namespace SymbolicImplicationVerification.Converts.Tokens.Operands
{
    internal abstract class Operand : Token
    {
        #region Public abstract methods

        public abstract void TryGetOperand(out IntegerTypeTerm? result);

        public abstract void TryGetOperand(out LogicalTerm? result);

        public abstract void TryGetOperand(out Formula? result);

        public abstract void TryGetOperand(out Program? result);

        #endregion
    }
}
