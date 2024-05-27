using SymImply.Converts.Tokens.Operands;
using SymImply.Formulas;

namespace SymImply.Converts.Tokens.Operators
{
    internal class DisjunctionOperand : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the DisjunctionOperand class.
        /// </summary>
        private static DisjunctionOperand? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private DisjunctionOperand() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular DisjunctionOperand instance.</returns>
        public static DisjunctionOperand Instance()
        {
            if (instance is null)
            {
                instance = new DisjunctionOperand();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "\\vee";
        }

        /// <summary>
        /// Returns the precedence of the disjunction operator.
        /// </summary>
        /// <returns>The precedence of the disjunction operator.</returns>
        public override int Precedence()
        {
            return disjunctionPrecedence;
        }

        /// <summary>
        /// Determines whether the disjunction operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the disjunction operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the disjunction operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the disjunction operator is left associative.
        /// </returns>
        public override bool RightAssociative()
        {
            return false;
        }

        /// <summary>
        /// Creates an evaluated token from the given left and right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the binary operation.</param>
        /// <param name="rightOperand">The right operand of the binary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public override FormulaOperand Evaluated(Operand leftOperand, Operand rightOperand)
        {
            return FormulaEvaluated(
                leftOperand, rightOperand,
                (left, right) => new DisjunctionFormula(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
