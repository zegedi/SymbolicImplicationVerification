﻿using System;
using SymImply.Formulas.Operations;
using SymImply.Types;

namespace SymImply.Formulas
{
    public class ImplicationFormula : BinaryOperationFormula
    {
        #region Constructors

        public ImplicationFormula(ImplicationFormula implication) : base(
            implication.identifier, 
            implication.leftOperand .DeepCopy(), 
            implication.rightOperand.DeepCopy()) { }

        public ImplicationFormula(Formula leftOperand, Formula rightOperand)
            : base(null, leftOperand, rightOperand) { }

        public ImplicationFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} \\rightarrow {1}", leftOperand, rightOperand);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftOperand, rightOperand) switch
        {
            (NotEvaluable, _            ) => NotEvaluable.Instance(),
            (_           , NotEvaluable ) => NotEvaluable.Instance(),
            (FALSE       , _            ) => TRUE.Instance(),
            (_           , TRUE         ) => TRUE.Instance(),
            (Formula left, FALSE        ) => ~left.DeepCopy(),
            (TRUE        , Formula right) => right.Evaluated(),
            (Formula left, Formula right) => 
                ReturnOrDeepCopy(new ImplicationFormula(left.DeepCopy(), right.DeepCopy())) 
        };

        /// <summary>
        /// Gets the linear operands of the given operation.
        /// </summary>
        /// <returns>The linear operands of the given operation.</returns>
        public override LinkedList<Formula> LinearOperands()
        {
            return LinearOperands(binary => binary is ImplicationFormula);
        }

        /// <summary>
        /// Gets the recursive linear operands of the given operation.
        /// </summary>
        /// <returns>The recursive linear operands of the given operation.</returns>
        public override LinkedList<Formula> RecursiveLinearOperands()
        {
            return LinearOperands(binary => binary is ImplicationFormula, true);
        }

        /// <summary>
        /// Gets the simplified linear operands of the given operation.
        /// </summary>
        /// <returns>The simplified linear operands of the given operation.</returns>
        public override LinkedList<Formula> SimplifiedLinearOperands()
        {
            return LinearOperands();
        }

        /// <summary>
        /// Binarize the given formulas.
        /// </summary>
        /// <param name="formulas">The list of formulas.</param>
        /// <returns>The result of the operation.</returns>
        public override ImplicationFormula Binarize(LinkedList<Formula> formulas)
        {
            ImplicationFormula? result = Binarize(
                formulas, (first, second) => new ImplicationFormula(first, second));

            if (result is null)
            {
                throw new ArgumentException();
            }

            return result;
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
            return obj is ImplicationFormula other &&
                   leftOperand .Equals(other.leftOperand) &&
                   rightOperand.Equals(other.rightOperand);
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
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new DisjunctionFormula(leftOperand.DeepCopy(), ~rightOperand.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current implication program.
        /// </summary>
        /// <returns>The created deep copy of the implication program.</returns>
        public override ImplicationFormula DeepCopy()
        {
            return new ImplicationFormula(this);
        }

        #endregion
    }
}
