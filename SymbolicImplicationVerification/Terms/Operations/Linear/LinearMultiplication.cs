using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public class LinearMultiplication : IntegerTypeLinearOperationTerm
    {
        public LinearMultiplication(IntegerType termType) : base(termType) { }

        public LinearMultiplication(LinkedList<IntegerTypeTerm> operandList, IntegerType termType)
            : base(operandList, termType) { }

        public LinearMultiplication(LinearMultiplication linear)
            : base(OperandListDeepCopy(linear.operandList), linear.termType.DeepCopy()) { }

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
            return obj is LinearMultiplication other &&
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
            const string multiplicationOperationSymbol = "*";

            return ToString(multiplicationOperationSymbol);
        }

        /// <summary>
        /// Create a deep copy of the current linear multiplication term.
        /// </summary>
        /// <returns>The created deep copy of the linear multiplication term.</returns>
        public override LinearMultiplication DeepCopy()
        {
            return new LinearMultiplication(this);
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
                    Summation                          => int.MaxValue - 2,
                    IntegerTypeLinearOperationTerm lin => lin.OperandList.Count(term => term is not IntegerConstant),
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
                return processedGroup;
            }

            IntegerTypeTerm processedOperand =
                nextOperand is IntegerTypeLinearOperationTerm operation ?
                operation.Evaluated() : nextOperand;

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
