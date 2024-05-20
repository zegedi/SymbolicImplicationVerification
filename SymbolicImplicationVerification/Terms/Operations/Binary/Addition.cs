using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Linear;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations.Binary
{
    public class Addition : IntegerTypeBinaryOperationTerm
    {
        #region Constant values

        private const int additiveNeutralElement = 0;
        private const int additiveInverseOfOne   = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the leftOperand of two rightOperand operands.
        /// </summary>
        /// <param name="leftConstant">The rightOperand left operand of the leftOperand.</param>
        /// <param name="rightConstant">The rightOperand right operand of the leftOperand.</param>
        public Addition(IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
            : this(leftConstant, rightConstant,
                   leftConstant.TermType.AdditionWithType(leftConstant.Value, rightConstant.Value)) { }

        /// <summary>
        /// Constructor for the leftOperand, where the left operand is a rightOperand.
        /// </summary>
        /// <param name="leftConstant">The rightOperand left operand of the leftOperand.</param>
        /// <param name="rightOperand">The right operand of the leftOperand.</param>
        public Addition(IntegerTypeConstant leftConstant, IntegerTypeTerm rightOperand)
            : this(leftConstant, rightOperand, rightOperand.TermType.AdditionWithType(leftConstant.Value)) { }

        /// <summary>
        /// Constructor for the leftOperand, where the right operand is a rightOperand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the leftOperand.</param>
        /// <param name="rightConstant">The rightOperand right operand of the leftOperand.</param>
        public Addition(IntegerTypeTerm leftOperand, IntegerTypeConstant rightConstant)
            : this(leftOperand, rightConstant, leftOperand.TermType.AdditionWithType(rightConstant.Value)) { }

        public Addition(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
            : this(leftOperand, rightOperand, leftOperand.TermType.AdditionWithType(rightOperand.TermType)) { }

        public Addition(Addition addition) : this(
            addition.leftOperand.DeepCopy(), addition.rightOperand.DeepCopy(), addition.termType.DeepCopy()) { }

        public Addition(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand, IntegerType termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current addition term.
        /// </summary>
        /// <returns>The created deep copy of the addition term.</returns>
        public override Addition DeepCopy()
        {
            return new Addition(this);
        }

        /// <summary>
        /// Creates a new addition instance.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The newly created addition.</returns>
        public override Addition CreateInstance(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0} + ", leftOperand.ToString());

            if (rightOperand is Addition or Subtraction)
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
            return obj is Addition other &&
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
            return obj is Addition addition &&
                   leftOperand .Matches(addition.leftOperand) &&
                   rightOperand.Matches(addition.rightOperand);
        }

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override IntegerTypeTerm Evaluated()
        {
            return Evaluated(
                result => PatternReplacer<IntegerType>.PatternsApplied(result, ReplacePatterns.CollapseGroups),
                result => PatternReplacer<IntegerType>.PatternsApplied(result, ReplacePatterns.LeftAssociateRules)
            );
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates a simplified version of the addition.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>The simplified verion of the addition.</returns>
        protected override IntegerTypeTerm Simplified(IntegerTypeTerm left, IntegerTypeTerm right) => (left, right) switch
        {
            (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
                => new IntegerTypeConstant(leftConstant.Value + rightConstant.Value),

            (IntegerTypeConstant constant, _) => constant.Value switch
            {
                < additiveNeutralElement => new Subtraction(right, new IntegerTypeConstant(-1 * constant.Value)),
                > additiveNeutralElement => new Addition(right, constant),
                _ => right
            },

            (_, Multiplication multip) => multip.LeftOperand switch
            {
                IntegerTypeConstant { Value: additiveInverseOfOne } =>
                    new Subtraction(left, multip.RightOperand),

                IntegerTypeConstant { Value: < additiveInverseOfOne } constant =>
                        new Subtraction(left, new IntegerTypeConstant(-1 * constant.Value) * multip.RightOperand),

                _ => new Addition(left, multip)
            },

            (_, IntegerTypeConstant constant) => constant.Value switch
            {
                < additiveNeutralElement => new Subtraction(left, new IntegerTypeConstant(-1 * constant.Value)),
                > additiveNeutralElement => new Addition(left, constant),
                _ => left
            },

            (Summation sum, _) => sum.AdditionWith(right),
            (_, Summation sum) => sum.AdditionWith(left),

            (_, _) => new Addition(left, right)
        };

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
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