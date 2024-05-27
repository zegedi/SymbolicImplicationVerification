using SymImply.Terms.Operations;
using SymImply.Terms.Operations.Binary;
using SymImply.Types;

namespace SymImply.Terms.Constants
{
    public class IntegerTypeConstant : Constant<int, IntegerType>
    {
        #region Constructors

        public IntegerTypeConstant(int value) : this(value, Integer.Instance()) { }

        public IntegerTypeConstant(int value, IntegerType type)
            : base(value, type) { }

        public IntegerTypeConstant(IntegerTypeConstant constant) 
            : this(constant.value, constant.termType.DeepCopy()) { }

        #endregion

        #region Implicit conversions

        public static implicit operator IntegerTypeConstant(int value)
        {
            return new IntegerTypeConstant(value);
        }

        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between a constant left and right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(IntegerTypeConstant leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between a constant left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(IntegerTypeConstant leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a constant left and right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(IntegerTypeConstant leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a constant left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(IntegerTypeConstant leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a constant left and right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(IntegerTypeConstant leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a constant left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(IntegerTypeConstant leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

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
            return obj is IntegerTypeConstant other && value == other.value;
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
        /// Create a deep copy of the current constant.
        /// </summary>
        /// <returns>The created deep copy of the constant.</returns>
        public override IntegerTypeConstant DeepCopy()
        {
            return new IntegerTypeConstant(this);
        }

        /// <summary>
        /// Evaluated the given constant, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override IntegerTypeConstant Evaluated()
        {
            return new IntegerTypeConstant(this);
        }

        #endregion
    }
}
