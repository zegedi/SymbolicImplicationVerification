using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Converts.Tokens.Operands
{
    internal class TermOperand : Operand
    {
        #region Fields

        private readonly IntegerTypeTerm? integerTypeTerm;

        private readonly LogicalTerm? logicalTerm;

        #endregion

        #region Constructors

        public TermOperand(IntegerTypeTerm term)
        {
            integerTypeTerm = term.DeepCopy();
        }

        public TermOperand(LogicalTerm term)
        {
            logicalTerm = term.DeepCopy();
        }

        #endregion

        #region Public properties

        public IntegerTypeTerm? IntegerTypeTerm
        {
            get { return integerTypeTerm; }
        }

        public LogicalTerm? LogicalTerm
        {
            get { return logicalTerm; }
        }

        #endregion

        #region Public methods

        public override string? ToString()
        {
            return integerTypeTerm?.ToString() ?? logicalTerm?.ToString();
        }

        public override void TryGetOperand(out IntegerTypeTerm? result)
        {
            result = integerTypeTerm?.DeepCopy();
        }

        public override void TryGetOperand(out LogicalTerm? result)
        {
            result = logicalTerm?.DeepCopy();
        }

        public override void TryGetOperand(out Formula? result)
        {
            result = logicalTerm is not null ? new LogicalTermFormula(logicalTerm.DeepCopy()) : null;
        }

        public override void TryGetOperand(out Program? result)
        {
            result = null;
        }

        #endregion
    }
}
