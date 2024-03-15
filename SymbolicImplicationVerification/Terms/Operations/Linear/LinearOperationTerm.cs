global using IntegerTypeLinearOperationTerm =
    SymbolicImplicationVerification.Terms.Operations.Linear.LinearOperationTerm<
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
        SymbolicImplicationVerification.Types.IntegerType>;

using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public abstract class LinearOperationTerm<OTerm, OType> : Term<OType>
        where OTerm : Term<OType>
        where OType : IntegerType
    {
        #region Fields

        protected LinkedList<OTerm> operandList;

        #endregion

        #region Constructors

        public LinearOperationTerm(OType termType) : base(termType)
        {
            operandList = new LinkedList<OTerm>();
        }

        public LinearOperationTerm(LinkedList<OTerm> operandList, OType termType) : base(termType)
        {
            this.operandList = operandList;
        }

        #endregion

        #region Public properties

        public virtual int Constant
        {
            get { return AccumulateConstants(); }
            set { }
        }

        public LinkedList<OTerm> OperandList
        {
            get { return operandList; }
            set { operandList = value; }
        }

        public static implicit operator OTerm(LinearOperationTerm<OTerm, OType> linearOperation)
        {
            return linearOperation;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            string result = String.Empty;

            foreach (var item in OperandList)
            {
                result = result + item.ToString() + ", ";
            }

            return result;
        }

        public OTerm Process()
        {
            OrderOperands();

            LinkedList<LinkedList<OTerm>> operandGroups = GroupOperands();

            LinkedList<OTerm> processedGroups = ProcessEachGroup(operandGroups);

            return AccumulateGroups(processedGroups);
        }

        public override string Hash(HashLevel level)
        {
            OrderOperands();

            switch (level)
            {
                case HashLevel.NO_CONSTANTS:

                    return string.Join("_", from   term in operandList
                                            where  term is not IntegerTypeConstant
                                            select term.Hash(level));

                default:

                    return string.Join("_", from   term in operandList
                                            select term.Hash(level));
            }
        }

        /*
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
            return obj is not null &&
                   obj is LinearOperationTerm<OTerm, OType> other &&
                   leftOperand.Equals(other.LeftOperand) &&
                   rightOperand.Equals(other.RightOperand);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        */

        #endregion

        #region Protected abstract methods

        protected abstract int AccumulateConstants();

        protected abstract void OrderOperands();

        protected abstract OTerm? ProcessNextOperand(OTerm? processedGroup, OTerm? nextOperand);

        protected abstract OTerm ProcessNextGroup(OTerm accumulated, OTerm nextGroup);


        /*
        public void Simplify()
        {
            Dictionary<string, (OTerm term, int constant)> equalHashes = new Dictionary<string, (OTerm, int)>();

            foreach (OTerm operand in operandList)
            {
                string hash = operand.Hash(HashLevel.NO_CONSTANTS);

                if (!equalHashes.ContainsKey(hash))
                {
                    equalHashes.Add(hash, (operand, 0));
                }

                (OTerm term, int constant) accumulated = equalHashes[hash];

                equalHashes[hash] = (
                    accumulated.term is not LinearOperationTerm<OTerm, OType> &&
                    operand is LinearOperationTerm<OTerm, OType> ? 
                    operand : accumulated.term,

                    accumulated.constant + 
                    (operand is LinearOperationTerm<OTerm, OType> linear ? linear.AccumulateConstants() : 1)
                );
            }

            LinkedList<OTerm> newOperandList = new LinkedList<OTerm>();

            foreach (KeyValuePair<string, (OTerm term, int constant)> operand in equalHashes)
            {
                if (operand.Value.constant == 0)
                {
                    continue;
                }

                if (operand.Value.term is LinearMultiplication linear)
                {
                    LinkedList<IntegerTpyeTerm> operands = new LinkedList<IntegerTpyeTerm>();

                    foreach (var op in linear.operandList)
                    {
                        if (op is not IntegerTypeConstant)
                        {
                            operands.AddLast(op);
                        }
                    }

                    if (operand.Value.constant != 1)
                    {

                    }
                }
            }
        }
        */

        #endregion

        #region Protected methods

        protected int AccumulateConstants(int neutralValue, Func<int, int, int> accumulate)
        {
            int resultValue = neutralValue;

            // Set the current node to the first.
            LinkedListNode<OTerm>? current = operandList.First;

            while (current is not null)
            {
                // Get the next one after the current one.
                LinkedListNode<OTerm>? next = current.Next;

                // Get the current term from the node.
                OTerm currentTerm = current.Value;

                // Test if the current term is a linear operation.
                if (currentTerm is LinearOperationTerm<OTerm, OType> linear)
                {
                    linear.AccumulateConstants();

                    if (linear.OperandList.Count == 1)
                    {
                        currentTerm = linear.OperandList.First();
                    }
                }

                // Test if the current term is a constant.
                if (currentTerm is IntegerTypeConstant constant)
                {
                    // Multiply with the constant's resultValue.
                    resultValue = accumulate(resultValue, constant.Value);

                    // Remove the current node from the list.
                    operandList.Remove(current);
                }

                current = next;
            }

            return resultValue;
        }

        protected void OrderOperands(Func<OTerm, int> keySelector)
        {
            AccumulateConstants();

            LinkedList<OTerm> sortedOperandList = new LinkedList<OTerm>();

            foreach (OTerm operand in operandList.OrderByDescending(keySelector)
                                                 .ThenBy(term => term.Hash(HashLevel.NO_CONSTANTS)))
            {
                sortedOperandList.AddLast(operand);
            }

            operandList = sortedOperandList;
        }

        protected LinkedList<LinkedList<OTerm>> GroupOperands()
        {
            Dictionary<string, LinkedList<OTerm>> equalHashes = new Dictionary<string, LinkedList<OTerm>>();

            foreach (OTerm operand in operandList)
            {
                string hash = operand.Hash(HashLevel.NO_CONSTANTS);

                if (!equalHashes.ContainsKey(hash))
                {
                    equalHashes.Add(hash, new LinkedList<OTerm>());
                }

                equalHashes[hash].AddLast(operand);
            }

            LinkedList<LinkedList<OTerm>> operandGroups = new LinkedList<LinkedList<OTerm>>();

            foreach (KeyValuePair<string, LinkedList<OTerm>> grouping in equalHashes)
            {
                operandGroups.AddLast(grouping.Value);
            }

            return operandGroups;
        }

        protected LinkedList<OTerm> ProcessEachGroup(LinkedList<LinkedList<OTerm>> groups)
        {
            LinkedList<OTerm> processedGroups = new LinkedList<OTerm>();

            foreach (LinkedList<OTerm> group in groups)
            {
                OTerm? processedGroup = null;

                foreach (OTerm operand in group)
                {
                    processedGroup = ProcessNextOperand(processedGroup, operand);
                }

                if (processedGroup is not null)
                {
                    processedGroups.AddLast(processedGroup);
                }
            }

            return processedGroups;
        }

        protected OTerm AccumulateGroups(LinkedList<OTerm> processedGroups)
        {
            OTerm accumulated = processedGroups.First();

            processedGroups.RemoveFirst();

            foreach (OTerm nextGroup in processedGroups)
            {
                accumulated = ProcessNextGroup(accumulated, nextGroup);
            }

            return accumulated;
        }

        #endregion
    }
}
