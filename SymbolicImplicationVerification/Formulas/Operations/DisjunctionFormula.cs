using System;
using System.Collections.Generic;
using SymbolicImplicationVerification.Formulas.Operations;
using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class DisjunctionFormula : BinaryOperationFormula
    {
        #region Constructors

        public DisjunctionFormula(DisjunctionFormula disjunction) : base(
            disjunction.identifier, 
            disjunction.leftOperand .DeepCopy(), 
            disjunction.rightOperand.DeepCopy()) { }

        public DisjunctionFormula(Formula leftOperand, Formula rightOperand)
            : base(null, leftOperand, rightOperand) { }

        public DisjunctionFormula(string? identifier, Formula leftOperand, Formula rightOperand)
            : base(identifier, leftOperand, rightOperand) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            bool addLeftParenthesis = typeof(QuantifiedFormula<>).IsAssignableFrom(leftOperand.GetType());

            return string.Format(
                addLeftParenthesis ? "({0}) \\vee {1}" : "{0} \\vee {1}", leftOperand, rightOperand);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftOperand, rightOperand) switch
        {
            (NotEvaluable, _            ) => NotEvaluable.Instance(),
            (_           , NotEvaluable ) => NotEvaluable.Instance(),
            (TRUE        , _            ) => TRUE.Instance(),
            (_           , TRUE         ) => TRUE.Instance(),
            (Formula left, FALSE        ) => left .Evaluated(),
            (FALSE       , Formula right) => right.Evaluated(),
            (Formula left, Formula right) => ReturnOrDeepCopy(left.Evaluated().DisjunctionWith(right.Evaluated()))
        };

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
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is DisjunctionFormula other &&
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

        public override LinkedList<Formula> LinearOperands()
        {
            return LinearOperands(binary => binary is DisjunctionFormula);
        }

        public override LinkedList<Formula> RecursiveLinearOperands()
        {
            return LinearOperands(binary => binary is DisjunctionFormula, true);
        }

        public override LinkedList<Formula> SimplifiedLinearOperands()
        {
            return SimplifiedLinearOperands<IntegerType>(
                (first, second) => first.DisjunctionWith(second),
                (first, second, formula) => formula is not DisjunctionFormula
            );
        }

        public override DisjunctionFormula Binarize(LinkedList<Formula> formulas)
        {
            DisjunctionFormula? result = Binarize(
                formulas, (first, second) => new DisjunctionFormula(first, second));

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
            return new ConjunctionFormula(leftOperand.Negated(), rightOperand.Negated());
        }

        /// <summary>
        /// Creates a deep copy of the current disjunction program.
        /// </summary>
        /// <returns>The created deep copy of the disjunction program.</returns>
        public override DisjunctionFormula DeepCopy()
        {
            return new DisjunctionFormula(this);
        }

        #endregion
    }
}
