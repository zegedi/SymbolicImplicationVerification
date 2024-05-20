using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Programs;

namespace SymbolicImplicationVerification.Converts.Tokens.Operands
{
    internal abstract class Operand : Token
    {
        #region Public abstract methods

        public abstract bool TryGetOperand(out IntegerTypeTerm? result);

        public abstract bool TryGetOperand(out LogicalTerm? result);

        public abstract bool TryGetOperand(out Formula? result);

        public abstract bool TryGetOperand(out Program? result);

        #endregion
    }
}
