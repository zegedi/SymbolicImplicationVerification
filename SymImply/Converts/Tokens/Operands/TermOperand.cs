using SymImply.Formulas;
using SymImply.Programs;
using SymImply.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymImply.Converts.Tokens.Operands
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

        public override bool TryGetOperand(out IntegerTypeTerm? result)
        {
            result = integerTypeTerm?.DeepCopy();

            return result is not null;
        }

        public override bool TryGetOperand(out LogicalTerm? result)
        {
            result = logicalTerm?.DeepCopy();

            return result is not null;
        }

        public override bool TryGetOperand(out Formula? result)
        {
            result = logicalTerm is not null ? new LogicalTermFormula(logicalTerm.DeepCopy()) : null;

            return result is not null;
        }

        public override bool TryGetOperand(out Program? result)
        {
            result = null;

            return false;
        }

        #endregion
    }
}
