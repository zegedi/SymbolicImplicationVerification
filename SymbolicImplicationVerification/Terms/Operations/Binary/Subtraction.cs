using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations.Linear;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations
{
    public class Subtraction : IntegerTypeBinaryOperationTerm
    {
        #region Constant values

        private const int additiveNeutralElement = 0;
        private const int additiveInverseOfOne = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the subtraction of two constant operands.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the subtraction.</param>
        /// <param name="rightConstant">The constant right operand of the subtraction.</param>
        public Subtraction(IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
            : this(leftConstant, rightConstant,
                   leftConstant.TermType.SubtractionWithType(leftConstant.Value, rightConstant.Value)) { }

        /// <summary>
        /// Constructor for the subtraction, where the left operand is a constant.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        public Subtraction(IntegerTypeConstant leftConstant, IntegerTypeTerm rightOperand)
            : this(leftConstant, rightOperand, rightOperand.TermType.SubtractionWithType(leftConstant.Value)) { }

        /// <summary>
        /// Constructor for the subtraction, where the right operand is a constant.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightConstant">The constant right operand of the subtraction.</param>
        public Subtraction(IntegerTypeTerm leftOperand, IntegerTypeConstant rightConstant)
            : this(leftOperand, rightConstant, leftOperand.TermType.SubtractionWithType(rightConstant.Value)) { }

        public Subtraction(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
            : this(leftOperand, rightOperand, leftOperand.TermType.SubtractionWithType(rightOperand.TermType)) { }

        public Subtraction(Subtraction subtraction)
            : this(subtraction.leftOperand .DeepCopy(),
                   subtraction.rightOperand.DeepCopy(), subtraction.termType.DeepCopy()) { }

        public Subtraction(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand, IntegerType termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between a subtraction left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Subtraction leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between a subtraction left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Subtraction leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a subtraction left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Subtraction leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a subtraction left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Subtraction leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a subtraction left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Multiplication operator *(Subtraction leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a subtraction left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Multiplication operator *(Subtraction leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current subtraction term.
        /// </summary>
        /// <returns>The created deep copy of the subtraction term.</returns>
        public override Subtraction DeepCopy()
        {
            return new Subtraction(this);
        }

        public override IntegerTypeBinaryOperationTerm
            CreateInstance(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0} - ", leftOperand.ToString());

            if (rightOperand is IntegerTypeBinaryOperationTerm)
            {
                stringBuilder.AppendFormat("({0})", rightOperand.ToString());
            }
            else
            {
                stringBuilder.Append(rightOperand.ToString());
            }

            return stringBuilder.ToString();
        }

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
            return obj is Subtraction other &&
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
        public override bool Matches(object? obj)
        {
            return obj is Subtraction subtraction &&
                   leftOperand .Matches(subtraction.leftOperand) &&
                   rightOperand.Matches(subtraction.rightOperand);
        }

        public override IntegerTypeTerm Evaluated()
        {
            return Evaluated(
                result => PatternReplacer<IntegerType>.PatternsApplied(result, ReplacePatterns.CollapseGroups),
                result => PatternReplacer<IntegerType>.PatternsApplied(result, ReplacePatterns.LeftAssociateRules)
            );
        }

        #endregion

        #region Protected methods

        protected override IntegerTypeTerm Simplified(IntegerTypeTerm left, IntegerTypeTerm right) => (left, right) switch
        {
            (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
                => new IntegerConstant(leftConstant.Value - rightConstant.Value),

            (_, Multiplication mult) => mult.LeftOperand switch
            {
                IntegerTypeConstant { Value: additiveInverseOfOne } =>
                        new Addition(left, mult.RightOperand),

                IntegerTypeConstant { Value: < additiveInverseOfOne } constant =>
                        new Addition(left, (IntegerConstant)(-1 * constant.Value) * mult.RightOperand),

                _ => new Subtraction(left, mult)
            },

            (IntegerTypeConstant constant, _) => constant.Value switch
            {
                additiveNeutralElement => new Multiplication(new IntegerConstant(additiveInverseOfOne), right),
                _ => new Subtraction(constant, right)
            },

            (_, IntegerTypeConstant constant) => constant.Value switch
            {
                additiveNeutralElement => left,
                _ => new Subtraction(left, constant)
            },

            (_, _) => left.Equals(right) switch
            {
                true => new IntegerConstant(additiveNeutralElement),
                _ => new Subtraction(left, right)
            }
        };

        protected override IntegerTypeLinearOperationTerm Linearized()
        {
            return Linearized(
                operation => {

                    // Expand all parenthesis.
                    IntegerTypeTerm expanded
                        = PatternReplacer<IntegerType>.PatternsApplied(operation, ReplacePatterns.ExpandRules);

                    // Convert all subtractions into multiplications and additions.
                    IntegerTypeTerm subtractionsConverted
                        = PatternReplacer<IntegerType>.PatternsApplied(expanded, ReplacePatterns.ConvertSubtractions);

                    return subtractionsConverted;
                },
                nextInProcess => nextInProcess is Multiplication,
                (operandList, termType) => new LinearAddition(operandList, termType.DeepCopy())
            );
        }

        #endregion
    }
}
