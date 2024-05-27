using System;
using System.Runtime.ExceptionServices;
using System.Text;
using SymImply.Formulas.Operations;
using SymImply.Formulas.Quantified;
using SymImply.Formulas.Relations;
using SymImply.Terms.Constants;
using SymImply.Terms.Variables;
using SymImply.Types;

namespace SymImply.Formulas
{
    public class ConjunctionFormula : BinaryOperationFormula
    {
        #region Constructors

        public ConjunctionFormula(ConjunctionFormula conjunction) : base(
            conjunction.identifier, 
            conjunction.leftOperand .DeepCopy(), 
            conjunction.rightOperand.DeepCopy()) { }

        public ConjunctionFormula(Formula leftOperand, Formula rightOperand)
            : base(null, leftOperand, rightOperand) { }

        public ConjunctionFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            bool addLeftParenthesis =
                typeof(QuantifiedFormula<>).IsAssignableFrom(leftOperand.GetType());

            return string.Format(
                addLeftParenthesis ? "({0}) \\wedge {1}" : "{0} \\wedge {1}", leftOperand, rightOperand);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftOperand, rightOperand) switch
        {
            (NotEvaluable, _            ) => NotEvaluable.Instance(),
            (_           , NotEvaluable ) => NotEvaluable.Instance(),
            (FALSE       , _            ) => FALSE.Instance(),
            (_           , FALSE        ) => FALSE.Instance(),
            (Formula left, TRUE         ) => left .Evaluated(),
            (TRUE        , Formula right) => right.Evaluated(),
            (Formula left, Formula right) => ReturnOrDeepCopy(left.Evaluated().ConjunctionWith(right.Evaluated()))
        };

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
            return obj is ConjunctionFormula other &&
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
        /// Gets the linear operands of the given operation.
        /// </summary>
        /// <returns>The linear operands of the given operation.</returns>
        public override LinkedList<Formula> LinearOperands()
        {
            return LinearOperands(binary => binary is ConjunctionFormula); 
        }

        /// <summary>
        /// Gets the recursive linear operands of the given operation.
        /// </summary>
        /// <returns>The recursive linear operands of the given operation.</returns>
        public override LinkedList<Formula> RecursiveLinearOperands()
        {
            return LinearOperands(binary => binary is ConjunctionFormula, true);
        }

        /// <summary>
        /// Gets the simplified linear operands of the given operation.
        /// </summary>
        /// <returns>The simplified linear operands of the given operation.</returns>
        public override LinkedList<Formula> SimplifiedLinearOperands()
        {
            return SimplifiedLinearOperands<IntegerType>(
                (first, second) => first.ConjunctionWith(second),
                (first, second, formula) => !formula.Equals(first.DeepCopy() & second.DeepCopy()) &&
                                            !formula.Equals(second.DeepCopy() & first.DeepCopy())
            );
        }

        /// <summary>
        /// Calcualtes the simplified verions of the formula.
        /// </summary>
        /// <returns>The simplified verions of the formula.</returns>
        public Formula Simplified()
        {
            LinkedList<Formula> simplifiedOperands = SimplifiedLinearOperands();

            return simplifiedOperands.Count switch
            {
                0 => TRUE.Instance(),
                1 => simplifiedOperands.First().DeepCopy(),
                _ => Binarize(simplifiedOperands)
            };
        }

        /// <summary>
        /// Binarize the given formulas.
        /// </summary>
        /// <param name="formulas">The list of formulas.</param>
        /// <returns>The result of the operation.</returns>
        public override ConjunctionFormula Binarize(LinkedList<Formula> formulas)
        {
            ConjunctionFormula? result = Binarize(
                formulas, (first, second) => new ConjunctionFormula(first, second));

            if (result is null)
            {
                throw new ArgumentException();
            }

            return result;
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new DisjunctionFormula(leftOperand.Negated(), rightOperand.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current conjunction program.
        /// </summary>
        /// <returns>The created deep copy of the conjunction program.</returns>
        public override ConjunctionFormula DeepCopy()
        {
            return new ConjunctionFormula(this);
        }

        #endregion
    }
}
