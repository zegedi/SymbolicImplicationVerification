using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public class LinearMultiplication : IntegerTypeLinearOperationTerm
    {
        public LinearMultiplication(IntegerType termType) : base(termType) { }

        public LinearMultiplication(LinkedList<IntegerTypeTerm> operandList, IntegerType termType)
            : base(operandList,termType) { }

        #region Public properties

        public override int Constant
        {
            get { return AccumulateConstants(); }
            set
            {
                AccumulateConstants();

                const int multiplicationNeutralValue = 1;

                if (operandList.First is not null &&
                    operandList.First.Value is IntegerTypeConstant constant)
                {
                    constant.Value = value;
                }
                else if (value != multiplicationNeutralValue)
                {
                    operandList.AddFirst(new IntegerConstant(value));
                }
            }
        }

        #endregion

        #region Protected methods

        protected override void OrderOperands()
        {
            OrderOperands((IntegerTypeTerm term) =>
            {
                return term switch
                {
                    IntegerConstant                    => int.MaxValue,
                    ChiFunction                        => int.MaxValue - 1,
                    Summation<IntegerType>             => int.MaxValue - 2,
                    IntegerTypeLinearOperationTerm lin => lin.OperandList.Count + 1,
                    Variable<IntegerType>              => 1,
                    _                                  => 0,
                };
            });
        }

        protected override int AccumulateConstants()
        {
            const int multiplicationNeutralValue = 1;

            int result = AccumulateConstants(
                multiplicationNeutralValue,
                (accumulated, currentValue) => accumulated * currentValue
            );

            operandList.AddFirst(new IntegerConstant(result));

            return result;
        }

        protected override IntegerTypeTerm?
            ProcessNextOperand(IntegerTypeTerm? processedGroup, IntegerTypeTerm? nextOperand)
        {
            if (nextOperand is null)
            {
                return processedGroup!;
            }

            IntegerTypeTerm processedOperand =
                nextOperand is IntegerTypeLinearOperationTerm operation ?
                operation.Process() : nextOperand;

            return processedGroup is not null ?
                   new Multiplication(processedGroup, processedOperand) : processedOperand;
        }


        protected override IntegerTypeTerm
            ProcessNextGroup(IntegerTypeTerm accumulated, IntegerTypeTerm nextGroup)
        {
            return new Multiplication(accumulated, nextGroup);
        }

        #endregion
    }
}
