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

        public static implicit operator BinaryOperationTerm<TypeTerm, Type>(BinaryOperationTerm<OTerm, OType> operation)
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

        /*
        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool Matches(object? obj);
        */

        /// <summary>
        /// Create a deep copy of the current binary operation term.
        /// </summary>
        /// <returns>The created deep copy of the binary operation term.</returns>
        public override abstract BinaryOperationTerm<OTerm, OType> DeepCopy();

        /// <summary>
        /// Creates an evaluated version of the binary operation.
        /// </summary>
        /// <returns>The evaluated version of the binary operation.</returns>
        public abstract OTerm Evaluated(OTerm left, OTerm right);

        public abstract OTerm Simplified();

        public abstract LinearOperationTerm<OTerm, OType> Linearized();

        public abstract BinaryOperationTerm<OTerm, OType> CreateInstance(OTerm leftOperand, OTerm rightOperand);

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
            return obj is BinaryOperationTerm<OTerm, OType> other &&
                   leftOperand .Equals(other.LeftOperand) &&
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

        public override string Hash(HashLevel level)
        {
            return leftOperand.Hash(level) + "_" + rightOperand.Hash(level);
        }

        /*
        public OTerm Simplified()
        {
            OTerm result = Evaluated();

            if (result is BinaryOperationTerm<OTerm, OType> operation)
            {
                // Linearize the 
                LinearOperationTerm<OTerm, OType> linearized = operation.Linearize();

                result = linearized.Process();

                result = PatternReplacer.PatternsApplied(result, Evaluations.Patterns.CollapseGroups);

                result = PatternReplacer.PatternsApplied(result, Evaluations.Patterns.LeftAssociateRules);
            }

            return result;
        }*/

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates a simplified version of the binary operation.
        /// </summary>
        /// <param name="ConstantSimplification">How to simplify two constant operands.</param>
        /// <param name="TermSimplification">Otherwise how to simplify the operands.</param>
        /// <returns>The simplified verion of the binary operation.</returns>
        public OTerm Evaluated()
        {
            OTerm left =
                leftOperand is BinaryOperationTerm<OTerm, OType> leftOperation ?
                leftOperation.Evaluated() : (OTerm) leftOperand.DeepCopy();

            OTerm right =
                rightOperand is BinaryOperationTerm<OTerm, OType> rightOperation ?
                rightOperation.Evaluated() : (OTerm) rightOperand.DeepCopy();

            return Evaluated(left, right);
        }

        /*
        /// <summary>
        /// Creates a simplified version of the binary operation.
        /// </summary>
        /// <param name="ConstantSimplification">How to simplify two constant operands.</param>
        /// <param name="TermSimplification">Otherwise how to simplify the operands.</param>
        /// <returns>The simplified verion of the binary operation.</returns>
        protected OTerm Evaluated(
            Func<IntegerTypeConstant, IntegerTypeConstant, OTerm> ConstantSimplification,
            Func<OTerm, OTerm, OTerm> TermSimplification)
        {
            OTerm left =
                leftOperand is BinaryOperationTerm<OTerm, OType> leftOperation ?
                leftOperation.Evaluated() : leftOperand;

            OTerm right =
                rightOperand is BinaryOperationTerm<OTerm, OType> rightOperation ?
                rightOperation.Evaluated() : rightOperand;

            if (left  is IntegerTypeConstant leftConstant &&
                right is IntegerTypeConstant rightConstant)
            {
                return ConstantSimplification(leftConstant, rightConstant);
            }

            return TermSimplification(left, right);
        }
        */

        /*
        protected LinearOperationTerm<OTerm,OType> Linearize(
            Func<Term<OType>, bool> appendOperands,
            Func<Term<OType>, bool> invertRightOperand,
            Func<Term<OType>, bool> appendLinearizedOperation )
        {
            LinkedList<OTerm> linearTerms = new LinkedList<OTerm>();

            return null;
        }
        */
        /*
        protected abstract LinearOperationTerm<OTerm, OType> Linearize();

        protected void Linearize(
            LinearOperationTerm<OTerm, OType> linearizedOperation,
            Func<BinaryOperationTerm<OTerm, OType>, bool> AppendOperands,
            Func<BinaryOperationTerm<OTerm, OType>, bool> IsInverseOperation,
            bool invertOperand,
            Func<OTerm, OTerm> OperandInverter )
        {
            const bool selectLeftOperand  = false;
            const bool selectRightOperand = true;

            foreach (bool rightOperandSelected in new bool[] { selectLeftOperand, selectRightOperand })
            {
                OTerm operand = rightOperandSelected ? rightOperand : leftOperand;

                BinaryOperationTerm<OTerm, OType>? operation = operand as BinaryOperationTerm<OTerm, OType>;

                if (operation is null)
                {
                    linearizedOperation.OperandList.AddLast(
                        invertOperand ? OperandInverter(operand) : operand);
                }
                else if (!AppendOperands(operation))
                {
                    linearizedOperation.OperandList.AddLast(operation.Linearize());
                }
                else if (rightOperandSelected && IsInverseOperation(operation))
                {
                    // kell-e invertálni
                }
                else
                {
                    operation.Linearize(
                        linearizedOperation, 
                        AppendOperands,
                        IsInverseOperation,
                        OperandInverter
                    );
                }
            }
        }
        */

        #endregion
    }
}
