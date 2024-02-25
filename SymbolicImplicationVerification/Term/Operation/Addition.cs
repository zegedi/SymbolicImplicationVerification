using SymbolicImplicationVerification.Type;
using SymbolicImplicationVerification.Term.Constant;
using System.ComponentModel.Design;

namespace SymbolicImplicationVerification.Term.Operation
{
    public class Addition : IntegerTypeBinaryOperationTerm, IExpression<Addition>
    {
        #region Constructors

        public Addition(IntegerTpyeTerm leftOperand, IntegerTpyeTerm rightOperand, Integer termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion

        #region Public methods

        public Addition Expand()
        {
            // Expand the left operand if it's an expression.
            IntegerTpyeTerm leftExpanded = leftOperand is IExpression<IntegerTypeBinaryOperationTerm> left ? left.Expand() : leftOperand;

            // Expand the right operand if it's an expression.
            IntegerTpyeTerm rightExpanded = rightOperand is IExpression<IntegerTypeBinaryOperationTerm> right ? right.Expand() : rightOperand;

            return new Addition(leftExpanded, rightExpanded, Integer.Instance());
        }



        private static IntegerType AdditionTypeSelector(IntegerTpyeTerm leftOperand, IntegerTpyeTerm rightOperand)
        {
            System.Type leftOperandRuntimeType  = leftOperand.TermType.GetType();
            System.Type rightOperandRuntimeType = rightOperand.TermType.GetType();

            bool operandsTypeEqual = leftOperandRuntimeType.Equals(rightOperandRuntimeType);

            if (operandsTypeEqual)
            {
                if (leftOperand.TermType is NaturalNumberType)
                {
                    if (leftOperand.TermType is PositiveInteger)
                    {
                        return PositiveInteger.Instance();
                    }
                    else
                    {
                        return NaturalNumber.Instance();
                    }
                }
                else 
                {
                    return Integer.Instance();
                }
            }
            else
            {
                if ()
            }

            if (operandsTypeEqual && ) { }

            if (leftOperand is BaseTerm<IntegerType>)
            {
                return NaturalNumber.Instance();
            }
            else if ()
            {
                return PositiveInteger.Instance();
            }


            else
            {
                return Integer.Instance();
            }
        }

        #endregion
    }
}