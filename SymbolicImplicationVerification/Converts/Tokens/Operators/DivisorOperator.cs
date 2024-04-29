﻿using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas.Relations;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal class DivisorOperator : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the DivisorOperator class.
        /// </summary>
        private static DivisorOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private DivisorOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular DivisorOperator instance.</returns>
        public static DivisorOperator Instance()
        {
            if (instance is null)
            {
                instance = new DivisorOperator();
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
            return "\\mid";
        }

        /// <summary>
        /// Returns the precedence of the divisor operator.
        /// </summary>
        /// <returns>The precedence of the divisor operator.</returns>
        public override int Precedence()
        {
            return relationalPrecedence;
        }

        /// <summary>
        /// Determines whether the divisor operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the divisor operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the divisor operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the divisor operator is left associative.
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
                leftOperand, rightOperand, (left, right) => new Divisor(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
