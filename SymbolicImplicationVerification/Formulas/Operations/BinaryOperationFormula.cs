using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;
using static System.Net.Mime.MediaTypeNames;

namespace SymbolicImplicationVerification.Formulas.Operations
{
    public abstract class BinaryOperationFormula : Formula
    {
        #region Fields

        protected Formula leftOperand;

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

        public Formula LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

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

        public abstract LinkedList<Formula> LinearOperands();

        public abstract LinkedList<Formula> RecursiveLinearOperands();

        public abstract LinkedList<Formula> SimplifiedLinearOperands();

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

        protected LinkedList<Formula> SimplifiedLinearOperands<T>(
            Func<Formula, Formula, Formula> simplify,
            Func<Formula, Formula, Formula, bool> resultPredicate) where T : Type
        {
            LinkedList<Formula> operands = LinearOperands();

            LinkedListNode<Formula>? currentNode = operands.First;
            LinkedListNode<Formula>? nextNode = currentNode?.Next;

            //while (currentNode is not null)
            //{
            //    if (currentNode.Value is BinaryRelationFormula<T> current)
            //    {
            //        while (nextNode is not null)
            //        {
            //            if (nextNode.Value is BinaryRelationFormula<T> next)
            //            {
            //                Formula result = simplify(current, next);

            //                if (resultPredicate(result) && currentNode is not null)
            //                {
            //                    operands.AddAfter(currentNode, result);

            //                    operands.Remove(currentNode);
            //                    operands.Remove(nextNode);

            //                    operands.AddFirst(result);

            //                    currentNode = operands.First;
            //                    nextNode    = currentNode;
            //                }
            //            }

            //            nextNode = nextNode?.Next;
            //        }
            //    }

            //    currentNode = currentNode?.Next;
            //    nextNode    = currentNode?.Next;
            //}

            //while (currentNode is not null)
            //{
            //    while (nextNode is not null)
            //    {
            //        if (currentNode!.Value is BinaryRelationFormula<T> current &&
            //            nextNode.Value     is BinaryRelationFormula<T> next)
            //        {
            //            Formula result = simplify(current, next);

            //            if (resultPredicate(result))
            //            {
            //                LinkedListNode<Formula> resultNode
            //                    = operands.AddAfter(currentNode, result);

            //                operands.Remove(currentNode);
            //                operands.Remove(nextNode);

            //                currentNode = resultNode;
            //                nextNode    = resultNode;
            //            }
            //        }

            //        nextNode = nextNode.Next;
            //    }

            //    currentNode = currentNode.Next;
            //    nextNode    = currentNode?.Next;
            //}

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
