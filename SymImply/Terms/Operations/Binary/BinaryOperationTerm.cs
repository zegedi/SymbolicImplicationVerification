global using TypeBinaryOperationTerm =
    SymImply.Terms.Operations.Binary.BinaryOperationTerm<
        SymImply.Terms.Term<SymImply.Types.Type>,
        SymImply.Types.Type>;

global using IntegerTypeBinaryOperationTerm =
    SymImply.Terms.Operations.Binary.BinaryOperationTerm<
        SymImply.Terms.Term<SymImply.Types.IntegerType>,
        SymImply.Types.IntegerType>;


using SymImply.Evaluations;
using SymImply.Formulas;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations.Linear;
using SymImply.Types;

namespace SymImply.Terms.Operations.Binary
{
    public abstract class BinaryOperationTerm<OTerm, OType> : Term<OType> //, IMatch
        where OTerm : Term<OType>
        where OType : Type
    {
        #region Fields

        /// <summary>
        /// The left operand of the binary operation.
        /// </summary>
        protected OTerm leftOperand;

        /// <summary>
        /// The right operand of the binary operation.
        /// </summary>
        protected OTerm rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationTerm(OTerm leftOperand, OTerm rightOperand, OType termType) : base(termType)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the left operand of the binary operation.
        /// </summary>
        public OTerm LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

        /// <summary>
        /// Gets or sets the right operand of the binary operation.
        /// </summary>
        public OTerm RightOperand
        {
            get { return rightOperand; }
            set { rightOperand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current binary operation term.
        /// </summary>
        /// <returns>The created deep copy of the binary operation term.</returns>
        public override abstract BinaryOperationTerm<OTerm, OType> DeepCopy();

        /// <summary>
        /// Creates an instance of the current binary operation.
        /// </summary>
        /// <param name="leftOperand">The left operand of the binary operation.</param>
        /// <param name="rightOperand">The right operand of the binary operation.</param>
        /// <returns>The newly created binary operatin.</returns>
        public abstract BinaryOperationTerm<OTerm, OType> CreateInstance(OTerm leftOperand, OTerm rightOperand);

        #endregion

        #region Public methods

        /// <summary>
        /// Detemines wheter the given <see cref="object"/> can be rearranged, to eqault the current binary operation.
        /// </summary>
        /// <param name="other">The other <see cref="object"/> to rearrange.</param>
        /// <returns>The result of the operation.</returns>
        public bool RearrangementEquals(object? other)
        {
            return other is BinaryOperationTerm<OTerm, OType> operation &&
                   Linearized().Equals(operation.Linearized());
        }

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            return leftOperand.Hash(level) + "_" + rightOperand.Hash(level);
        }

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <param name="collapseGroups">How to collapse the groups.</param>
        /// <param name="associateGroups">How to associate the groups.</param>
        /// <returns>The newly created instance of the result.</returns>
        public Term<OType> Evaluated(
            Func<Term<OType>, Term<OType>> collapseGroups,
            Func<Term<OType>, Term<OType>> associateGroups)
        {
            Term<OType> result = Simplified();

            if (result is BinaryOperationTerm<OTerm, OType> operation)
            {
                LinearOperationTerm<OTerm, OType> linearized = operation.Linearized();

                result = linearized.Evaluated();

                result = collapseGroups(result);

                if (result is BinaryOperationTerm<OTerm, OType> operationTerm)
                {
                    result = operationTerm.Simplified();

                    result = associateGroups(result);
                }
            }

            return result;
        }

        #endregion

        #region Protected abstract methods

        /// <summary>
        /// Creates a simplified version of the binary operation.
        /// </summary>
        /// <returns>The simplified version of the binary operation.</returns>
        protected abstract Term<OType> Simplified(Term<OType> leftOperand, Term<OType> rightOperand);

        /// <summary>
        /// Creates a linearized version of the binary operation.
        /// </summary>
        /// <returns>The linearized version of the binary operation.</returns>
        protected abstract LinearOperationTerm<OTerm, OType> Linearized();

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates a simplified version of the binary operation.
        /// </summary>
        /// <returns>The simplified verion of the binary operation.</returns>
        protected Term<OType> Simplified()
        {
            Term<OType> left =
                leftOperand is BinaryOperationTerm<OTerm, OType> leftOperation ?
                leftOperation.Simplified() : leftOperand.Evaluated();

            Term<OType> right =
                rightOperand is BinaryOperationTerm<OTerm, OType> rightOperation ?
                rightOperation.Simplified() : rightOperand.Evaluated();

            return Simplified(left, right);
        }

        /// <summary>
        /// Creates a linearized version of the binary operation.
        /// </summary>
        /// <param name="preprocessBinaryOperation">How to preprocess the binary operations.</param>
        /// <param name="linearizeBinaryOperation">How to linearise the binary operations.</param>
        /// <param name="createLinearOperation">How to create the linear operation.</param>
        /// <returns>The linearized version of the binary operation.</returns>
        protected LinearOperationTerm<OTerm, OType> Linearized(
            Func<Term<OType>, Term<OType>> preprocessBinaryOperation,
            Func<BinaryOperationTerm<OTerm, OType>, bool> linearizeBinaryOperation,
            Func<LinkedList<Term<OType>>, OType, LinearOperationTerm<OTerm, OType>> createLinearOperation)
        {
            LinkedList<Term<OType>> unprocessed = new LinkedList<Term<OType>>();
            LinkedList<Term<OType>> operandList = new LinkedList<Term<OType>>();

            unprocessed.AddLast(preprocessBinaryOperation(this));

            while (unprocessed.Count > 0)
            {
                Term<OType> nextInProcess = unprocessed.First();
                unprocessed.RemoveFirst();

                if (nextInProcess is BinaryOperationTerm<OTerm, OType> operation)
                {
                    if (linearizeBinaryOperation(operation))
                    {
                        operandList.AddLast(operation.Linearized());
                    }
                    else
                    {
                        unprocessed.AddLast(operation.leftOperand);
                        unprocessed.AddLast(operation.rightOperand);
                    }
                }
                else
                {
                    operandList.AddLast(nextInProcess);
                }
            }

            return createLinearOperation(operandList, termType);
        }

        #endregion
    }
}
