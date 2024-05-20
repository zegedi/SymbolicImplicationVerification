namespace SymbolicImplicationVerification.Formulas.Operations
{
    public abstract class BinaryOperationFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The left operand of the binary operation.
        /// </summary>
        protected Formula leftOperand;

        /// <summary>
        /// The right operand of the binary operation.
        /// </summary>
        protected Formula rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationFormula(Formula leftOperand, Formula rightOperand)
            : this(null, leftOperand, rightOperand) { }

        public BinaryOperationFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the left operand of the binary operation.
        /// </summary>
        public Formula LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

        /// <summary>
        /// Gets or sets the right operand of the binary operation.
        /// </summary>
        public Formula RightOperand
        {
            get { return rightOperand; }
            set { rightOperand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current binary operation program.
        /// </summary>
        /// <returns>The created deep copy of the binary operation program.</returns>
        public override abstract BinaryOperationFormula DeepCopy();

        /// <summary>
        /// Gets the linear operands of the given operation.
        /// </summary>
        /// <returns>The linear operands of the given operation.</returns>
        public abstract LinkedList<Formula> LinearOperands();

        /// <summary>
        /// Gets the recursive linear operands of the given operation.
        /// </summary>
        /// <returns>The recursive linear operands of the given operation.</returns>
        public abstract LinkedList<Formula> RecursiveLinearOperands();

        /// <summary>
        /// Gets the simplified linear operands of the given operation.
        /// </summary>
        /// <returns>The simplified linear operands of the given operation.</returns>
        public abstract LinkedList<Formula> SimplifiedLinearOperands();

        /// <summary>
        /// Binarize the given formulas.
        /// </summary>
        /// <param name="formulas">The list of formulas.</param>
        /// <returns>The result of the operation.</returns>
        public abstract BinaryOperationFormula Binarize(LinkedList<Formula> formulas);

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified program is equivalent to the current program.
        /// </summary>
        /// <param name="other">The program to compare with the current program.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            return Evaluated().Equals(other.Evaluated());
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Gets the linear operands of the given operation.
        /// </summary>
        /// <param name="predicate">Which operands to select.</param>
        /// <param name="recursive">Determines whether the operation is recursive or not.</param>
        /// <returns>The linear operands of the given operation.</returns>
        protected LinkedList<Formula> LinearOperands(
            Func<BinaryOperationFormula, bool> predicate, bool recursive = false)
        {
            LinkedList<Formula> unprocessed = new LinkedList<Formula>();

            unprocessed.AddLast(this);

            LinkedList<Formula> operands = new LinkedList<Formula>();

            while (unprocessed.Count > 0)
            {
                Formula operand = unprocessed.First();
                unprocessed.RemoveFirst();

                bool processOperands = recursive || !operand.HasIdentifier;

                if (operand is BinaryOperationFormula binary && predicate(binary) && processOperands)
                {
                    unprocessed.AddFirst(binary.rightOperand);
                    unprocessed.AddFirst(binary.leftOperand);
                }
                else
                {
                    operands.AddLast(operand.DeepCopy());
                }
            }

            return operands;
        }

        /// <summary>
        /// Returns the simplified linear operands.
        /// </summary>
        /// <param name="simplify">How to simplify the operands.</param>
        /// <param name="resultPredicate">Which operands to select.</param>
        /// <returns>The result of the operation.</returns>
        protected LinkedList<Formula> SimplifiedLinearOperands<T>(
            Func<Formula, Formula, Formula> simplify,
            Func<Formula, Formula, Formula, bool> resultPredicate) where T : Type
        {
            LinkedList<Formula> operands = LinearOperands();

            LinkedListNode<Formula>? currentNode = operands.First;
            LinkedListNode<Formula>? nextNode = currentNode?.Next;

            while (currentNode is not null)
            {
                while (nextNode is not null)
                {
                    Formula result = simplify(currentNode.Value, nextNode.Value);

                    if (resultPredicate(currentNode.Value, nextNode.Value, result))
                    {
                        LinkedListNode<Formula> resultNode = operands.AddAfter(currentNode, result);

                        operands.Remove(currentNode);
                        operands.Remove(nextNode);

                        currentNode = resultNode;
                        nextNode    = resultNode;
                    }

                    nextNode = nextNode.Next;
                }

                currentNode = currentNode.Next;
                nextNode    = currentNode?.Next;
            }

            return operands;
        }

        /// <summary>
        /// How to binarize the given linear formulas.
        /// </summary>
        /// <param name="formulas">The list of linear formulas.</param>
        /// <param name="binarize">How to binarize the formulas.</param>
        /// <returns>The result of the operation.</returns>
        protected T? Binarize<T>(
            LinkedList<Formula> formulas, Func<Formula, Formula, T> binarize) where T : BinaryOperationFormula
        {
            T? result = null;

            while (formulas.Count >= 2)
            {
                Formula first = formulas.First();
                formulas.RemoveFirst();

                Formula second = formulas.First();
                formulas.RemoveFirst();

                result = binarize(first, second);

                formulas.AddFirst(result);
            }

            return result;
        }

        #endregion
    }
}
