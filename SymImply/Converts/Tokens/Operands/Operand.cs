using SymImply.Formulas;
using SymImply.Programs;

namespace SymImply.Converts.Tokens.Operands
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
