using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.Variables;
using System.Data;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using System.ComponentModel.DataAnnotations;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public class LinearAddition : IntegerTypeLinearOperationTerm
    {
        #region Constructors

        public LinearAddition(IntegerType termType) : base(termType) { }

        public LinearAddition(LinkedList<IntegerTypeTerm> operandList, IntegerType termType)
            : base(operandList, termType) { }

        #endregion

        #region Public properties

        public override int Constant
        {
            get { return AccumulateConstants(); }
            set
            {
                AccumulateConstants();

                const int additionNeutralValue = 0;

                if (operandList.Last is not null &&
                    operandList.Last.Value is IntegerTypeConstant constant)
                {
                    constant.Value = value;
                }
                else if (value != additionNeutralValue)
                {
                    operandList.AddLast(new IntegerConstant(value));
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
                    ChiFunction                        => int.MaxValue,
                    Summation<IntegerType>             => int.MaxValue - 1,
                    IntegerTypeLinearOperationTerm lin => lin.OperandList.Count + 1,
                    Variable<IntegerType>              => 1,
                    _                                  => 0,
                };
            });
        }

        protected override int AccumulateConstants()
        {
            const int additionNeutralValue = 0;

            int result = AccumulateConstants(
                additionNeutralValue,
                (accumulated, currentValue) => accumulated + currentValue
            );

            if (result != additionNeutralValue || operandList.Count == 0)
            {
                operandList.AddLast(new IntegerConstant(result));
            }

            return result;
        }

        protected override IntegerTypeTerm? 
            ProcessNextOperand(IntegerTypeTerm? processedGroup, IntegerTypeTerm? nextOperand)
        {
            if (nextOperand is null)
            {
                return processedGroup!;
            }

            if (processedGroup is not null &&
                nextOperand is LinearMultiplication multiplication)
            {
                if (multiplication.Constant < 0)
                {
                    multiplication.Constant = -1 * multiplication.Constant;

                    return new Subtraction(processedGroup, multiplication.Process());
                }
                    
                return new Addition(processedGroup, multiplication.Process());
            }

            IntegerTypeTerm processedOperand =
                nextOperand is IntegerTypeLinearOperationTerm operation ?
                operation.Process() : nextOperand;

            return processedGroup is not null ?
                   new Addition(processedGroup, processedOperand) : processedOperand;
        }


        protected override IntegerTypeTerm
            ProcessNextGroup(IntegerTypeTerm accumulated, IntegerTypeTerm nextGroup)
        {
            return new Addition(accumulated, nextGroup);
        }

        /*
        public override void Simplify()
        {
            AccumulateConstants();

            Dictionary<IntegerTpyeTerm, >

            //operandList.OrderBy(term => term.ToString());
            operandList.OrderByDescending((IntegerTpyeTerm term) =>
            {
                return term switch
                {
                    IntegerTypeLinearOperationTerm linearOperation => linearOperation.OperandList.Count,
                    Summation<IntegerType> summation => double.MaxValue,
                    Variable<IntegerType> variable => 1.5,
                    ChiFunction chiFunction => 1.2,
                    _ => 1,
                };
            })
            .ThenByDescending(term => term.ToString());
        }
        */

        #endregion
    }
}
