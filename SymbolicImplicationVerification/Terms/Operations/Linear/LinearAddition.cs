using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.Variables;
using System.Data;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public class LinearAddition : IntegerTypeLinearOperationTerm
    {
        #region Constructors

        public LinearAddition(IntegerType termType) : base(termType) { }

        public LinearAddition(LinkedList<IntegerTypeTerm> operandList, IntegerType termType)
            : base(operandList, termType) { }

        public LinearAddition(LinearAddition linearAddition)
            : base(OperandListDeepCopy(linearAddition.operandList), linearAddition.termType.DeepCopy()) { }

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

        #region Public methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is LinearAddition other &&
                   operandList.Count == other.operandList.Count &&
                   operandList.All(other.operandList.Contains);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            const string additionOperationSymbol = "+";

            return ToString(additionOperationSymbol);
        }

        /// <summary>
        /// Create a deep copy of the current linear addition term.
        /// </summary>
        /// <returns>The created deep copy of the linear addition term.</returns>
        public override LinearAddition DeepCopy()
        {
            return new LinearAddition(this);
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
                    Summation                          => int.MaxValue - 1,
                    IntegerTypeLinearOperationTerm lin => lin.OperandList.Count(term => term is not IntegerConstant),
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
                return processedGroup;
            }

            if (processedGroup is not null && nextOperand is LinearMultiplication multiplication)
            {
                if (multiplication.Constant < 0)
                {
                    multiplication.Constant = -1 * multiplication.Constant;

                    return new Subtraction(processedGroup, multiplication.Evaluated());
                }
                    
                return new Addition(processedGroup, multiplication.Evaluated());
            }

            IntegerTypeTerm processedOperand =
                nextOperand is IntegerTypeLinearOperationTerm operation ?
                operation.Evaluated() : nextOperand;

            return processedGroup is not null ?
                   new Addition(processedGroup, processedOperand) : processedOperand;
        }


        protected override Addition ProcessNextGroup(IntegerTypeTerm accumulated, IntegerTypeTerm nextGroup)
        {
            return new Addition(accumulated, nextGroup);
        }

        #endregion
    }
}
