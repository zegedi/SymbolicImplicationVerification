using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Converts.Tokens.Operands
{
    internal class ProgramOperand : Operand
    {
        #region Fields

        private readonly Program program;

        #endregion

        #region Constructors

        public ProgramOperand(Program program)
        {
            this.program = program.DeepCopy();
        }

        #endregion

        #region Public properties

        public Program Program
        {
            get { return program; }
        }

        #endregion

        #region Public methods

        public override string? ToString()
        {
            return program.ToString();
        }

        public override bool TryGetOperand(out IntegerTypeTerm? result)
        {
            result = null;

            return false;
        }

        public override bool TryGetOperand(out LogicalTerm? result)
        {
            result = null;

            return false;
        }

        public override bool TryGetOperand(out Formula? result)
        {
            result = null;

            return false;
        }

        public override bool TryGetOperand(out Program? result)
        {
            result = program.DeepCopy();

            return true;
        }

        #endregion
    }
}
