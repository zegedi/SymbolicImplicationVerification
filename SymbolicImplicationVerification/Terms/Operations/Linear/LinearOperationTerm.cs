global using IntegerTypeLinearOperationTerm =
    SymbolicImplicationVerification.Terms.Operations.Linear.LinearOperationTerm<
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
        SymbolicImplicationVerification.Types.IntegerType>;

using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations.Linear
{
    public abstract class LinearOperationTerm<OTerm, OType> : Term<OType>
        where OTerm : Term<OType>
        where OType : Type
    {
        #region Fields

        /// <summary>
        /// The list of operands.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the constant term.
        /// </summary>
        public virtual int Constant
        {
            get { return AccumulateConstants(); }
            set { }
        }

        /// <summary>
        /// Gets or sets the operand list.
        /// </summary>
        public LinkedList<OTerm> OperandList
        {
            get { return operandList; }
            set { operandList = value; }
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Creates a deep copy of the operand list.
        /// </summary>
        /// <param name="operandList">The operand list to copy.</param>
        /// <returns>The copy of the operand list.</returns>
        protected static LinkedList<OTerm> OperandListDeepCopy(LinkedList<OTerm> operandList)
        {
            LinkedList<OTerm> copyOperandList = new LinkedList<OTerm>();

            foreach (OTerm operand in operandList)
            {
                copyOperandList.AddLast((OTerm) operand.DeepCopy());
            }

            return copyOperandList;
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current linear operation term.
        /// </summary>
        /// <returns>The created deep copy of the linear operation term.</returns>
        public override abstract LinearOperationTerm<OTerm, OType> DeepCopy();

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
            return obj is LinearOperationTerm<OTerm, OType> other &&
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
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Term<OType> Evaluated()
        {
            OrderOperands();

            LinkedList<LinkedList<OTerm>> operandGroups = GroupOperands();

            LinkedList<Term<OType>> processedGroups = ProcessEachGroup(operandGroups);

            return AccumulateGroups(processedGroups);
        }

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
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

        #endregion

        #region Protected abstract methods

        /// <summary>
        /// Accumulates the constansts.
        /// </summary>
        /// <returns>The value of the accumulation.</returns>
        protected abstract int AccumulateConstants();

        /// <summary>
        /// Orders the operands.
        /// </summary>
        protected abstract void OrderOperands();

        /// <summary>
        /// Processes the next operand, than adds it to the group.
        /// </summary>
        /// <param name="processedGroup">The processed operand group.</param>
        /// <param name="nextOperand">The next operand to process.</param>
        /// <returns>The result of the process.</returns>
        protected abstract Term<OType>? ProcessNextOperand(Term<OType>? processedGroup, OTerm? nextOperand);

        /// <summary>
        /// Processes the next group, than adds it to the group.
        /// </summary>
        /// <param name="accumulated">The accumulated groups.</param>
        /// <param name="nextGroup">The next group to process.</param>
        /// <returns>The accumulated groups.</returns>
        protected abstract Term<OType> ProcessNextGroup(Term<OType> accumulated, Term<OType> nextGroup);

        #endregion

        #region Protected methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="operationSymbol">A symbol that represents the current linear operation.</param>
        /// <returns>A string that represents the current object.</returns>
        protected string ToString(string operationSymbol)
        {
            StringBuilder stringBuilder = new StringBuilder(string.Format("{0}(", operationSymbol));

            string delimiter = string.Empty;

            foreach (OTerm operand in operandList)
            {
                stringBuilder.AppendFormat("{0}{1}", delimiter, operand);

                delimiter = ", ";
            }

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Accumulates the constansts.
        /// </summary>
        /// <param name="neutralValue">The neutral value of the operation.</param>
        /// <param name="accumulate">The function to accumulate with.</param>
        /// <returns>The value of the accumulation.</returns>
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

        /// <summary>
        /// Orders the operands.
        /// </summary>
        /// <param name="keySelector">The function which is used for the ordering.</param>
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

        /// <summary>
        /// Groups the operands.
        /// </summary>
        /// <returns>The list of operand groups.</returns>
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

        /// <summary>
        /// Processes each group inside the list of groups.
        /// </summary>
        /// <param name="groups">The list of operand groups.</param>
        /// <returns>The list of processed groups.</returns>
        protected LinkedList<Term<OType>> ProcessEachGroup(LinkedList<LinkedList<OTerm>> groups)
        {
            LinkedList<Term<OType>> processedGroups = new LinkedList<Term<OType>>();

            foreach (LinkedList<OTerm> group in groups)
            {
                Term<OType>? processedGroup = null;

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

        /// <summary>
        /// Accumulates the groups.
        /// </summary>
        /// <param name="processedGroups">The list of processed groups.</param>
        /// <returns>The accumulated groups.</returns>
        protected Term<OType> AccumulateGroups(LinkedList<Term<OType>> processedGroups)
        {
            Term<OType> accumulated = processedGroups.First();

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
