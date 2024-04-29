using SymbolicImplicationVerification.Converts.Tokens.Operands;

namespace SymbolicImplicationVerification.Converts.Tokens.Functions
{
    internal abstract class FunctionCallStart : Token
    {
        #region Public abstract methods

        /// <summary>
        /// Creates an evaluated token from the operand.
        /// </summary>
        /// <param name="operand">The operand of the unary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public abstract Token Evaluated(Operand operand);

        #endregion

        #region Protected methods

        protected TermOperand Evaluated(Operand operand, Func<IntegerTypeTerm, LogicalTerm> createTerm)
        {
            operand.TryGetOperand(out IntegerTypeTerm? term);

            if (term is not null)
            {
                return new TermOperand(createTerm(term));
            }

            throw new Exception();
        }

        protected TermOperand Evaluated(Operand operand, Func<LogicalTerm, IntegerTypeTerm> createTerm)
        {
            operand.TryGetOperand(out LogicalTerm? term);

            if (term is not null)
            {
                return new TermOperand(createTerm(term));
            }

            throw new Exception();
        }

        #endregion
    }
}
