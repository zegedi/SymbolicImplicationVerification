using SymImply.Evaluations;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations.Linear;
using SymImply.Types;
using System.Text;

namespace SymImply.Terms.Operations.Binary
{
    public class Multiplication : IntegerTypeBinaryOperationTerm
    {
        #region Constant values

        private const int additiveNeutralElement = 0;
        private const int multiplicativeNeutralElement = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the multiplication of two constant operands.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the multiplication.</param>
        /// <param name="rightConstant">The constant right operand of the multiplication.</param>
        public Multiplication(IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
            : this(leftConstant, rightConstant,
                   leftConstant.TermType.MultiplicationWithType(leftConstant.Value, rightConstant.Value)) { }

        /// <summary>
        /// Constructor for the multiplication, where the left operand is a constant.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        public Multiplication(IntegerTypeConstant leftConstant, IntegerTypeTerm rightOperand)
            : this(leftConstant, rightOperand, 
                   rightOperand.TermType.MultiplicationWithType(leftConstant.Value)) { }

        /// <summary>
        /// Constructor for the multiplication, where the right operand is a constant.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightConstant">The constant right operand of the multiplication.</param>
        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeConstant rightConstant)
            : this(leftOperand, rightConstant, 
                   leftOperand.TermType.MultiplicationWithType(rightConstant.Value)) { }

        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
            : this(leftOperand, rightOperand, 
                   leftOperand.TermType.MultiplicationWithType(rightOperand.TermType)) { }

        public Multiplication(Multiplication multiplication)
            : this(multiplication.leftOperand .DeepCopy(),
                   multiplication.rightOperand.DeepCopy(), multiplication.termType.DeepCopy()) { }

        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand, IntegerType termType)
            : base(leftOperand, rightOperand, termType) { }


        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current multiplication term.
        /// </summary>
        /// <returns>The created deep copy of the multiplication term.</returns>
        public override Multiplication DeepCopy()
        {
            return new Multiplication(this);
        }

        /// <summary>
        /// Creates an instance of the current binary operation.
        /// </summary>
        /// <param name="leftOperand">The left operand of the binary operation.</param>
        /// <param name="rightOperand">The right operand of the binary operation.</param>
        /// <returns>The newly created binary operatin.</returns>
        public override Multiplication CreateInstance(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (leftOperand is IntegerTypeConstant { Value: -1 })
            {
                stringBuilder.Append('-');
            }
            else
            {
                string formatString = leftOperand is Addition or Subtraction ? "({0}) \\cdot " : "{0} \\cdot ";

                stringBuilder.AppendFormat(formatString, leftOperand);
            }

            if (rightOperand is IntegerTypeBinaryOperationTerm)
            {
                stringBuilder.AppendFormat("({0})", rightOperand);
            }
            else
            {
                stringBuilder.Append(rightOperand);
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
            return obj is Multiplication other &&
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
            return obj is Multiplication multiplication &&
                   leftOperand .Matches(multiplication.leftOperand) &&
                   rightOperand.Matches(multiplication.rightOperand);
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
        /// Creates a simplified version of the multiplication.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>The simplified verion of the multiplication.</returns>
        protected override IntegerTypeTerm Simplified(IntegerTypeTerm left, IntegerTypeTerm right) => (left, right) switch
        {
            (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
                => new IntegerTypeConstant(leftConstant.Value * rightConstant.Value),

            (IntegerTypeConstant constant, _) => constant.Value switch
            {
                multiplicativeNeutralElement => right,
                additiveNeutralElement => new IntegerTypeConstant(additiveNeutralElement),
                _ => new Multiplication(constant, right)
            },

            (_, IntegerTypeConstant constant) => constant.Value switch
            {
                multiplicativeNeutralElement => left,
                additiveNeutralElement => new IntegerTypeConstant(additiveNeutralElement),
                _ => new Multiplication(constant, left)
            },

            (_, _) => new Multiplication(left, right)
        };

        /// <summary>
        /// Creates a linearized version of the binary operation.
        /// </summary>
        /// <returns>The linearized version of the binary operation.</returns>
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
                nextInProcess => nextInProcess is Addition,
                (operandList, termType) => new LinearMultiplication(operandList, termType.DeepCopy())
            );
        }

        #endregion
    }
}
