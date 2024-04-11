global using TypeBinaryOperationTerm =
    SymbolicImplicationVerification.Terms.Operations.Binary.BinaryOperationTerm<
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>,
        SymbolicImplicationVerification.Types.Type>;

global using IntegerTypeBinaryOperationTerm =
    SymbolicImplicationVerification.Terms.Operations.Binary.BinaryOperationTerm<
        SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
        SymbolicImplicationVerification.Types.IntegerType>;


using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Linear;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Operations.Binary
{
    public abstract class BinaryOperationTerm<OTerm, OType> : Term<OType> //, IMatch
        where OTerm : Term<OType>
        where OType : Type
    {
        #region Fields

        protected OTerm leftOperand;
        protected OTerm rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationTerm(OTerm leftOperand, OTerm rightOperand, OType termType) : base(termType)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Implicit conversions

        public static implicit operator TypeBinaryOperationTerm(BinaryOperationTerm<OTerm, OType> operation)
        {
            return operation;
        }

        #endregion

        #region Public properties

        public OTerm LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

        public OTerm RightOperand
        {
            get { return rightOperand; }
            set { rightOperand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current binary operation term.
        /// </summary>
        /// <returns>The created deep copy of the binary operation term.</returns>
        public override abstract BinaryOperationTerm<OTerm, OType> DeepCopy();

        public abstract BinaryOperationTerm<OTerm, OType> CreateInstance(OTerm leftOperand, OTerm rightOperand);

        #endregion

        #region Public methods

        public bool RearrangementEquals(object? other)
        {
            return other is BinaryOperationTerm<OTerm, OType> operation &&
                   Linearized().Equals(operation.Linearized());
        }

        public override string Hash(HashLevel level)
        {
            return leftOperand.Hash(level) + "_" + rightOperand.Hash(level);
        }

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
