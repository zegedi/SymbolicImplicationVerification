using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public abstract class Equal<T> : BinaryRelationFormula<T> where T : Type
    {
        #region Constructors

        public Equal(Equal<T> equal) : base(
            equal.identifier, 
            equal.leftComponent .DeepCopy(), 
            equal.rightComponent.DeepCopy()) { }

        public Equal(Term<T> leftComponent, Term<T> rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public Equal(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} = {1}", leftComponent, rightComponent);
        }

        /// <summary>
        /// Try to substitute the assignment components into the formula.
        /// </summary>
        /// <param name="formula">The formula to substitute into.</param>
        /// <returns>The result of the substitution.</returns>
        public Formula? SubstituteVariable(Formula formula)
        {
            Formula substitutedFormula = formula.DeepCopy();

            LinkedList<EntryPoint<T>> entryPoints = new LinkedList<EntryPoint<T>>();

            Term<T> replaceTerm = leftComponent;

            List<(Term<T> leftComponent, Term<T> rightComponent)> pairs = new List<(Term<T>, Term<T>)>
            {
                (leftComponent, rightComponent),
                (rightComponent, leftComponent)
            };

            foreach ((Term<T> leftComponent, Term<T> rightComponent) pair in pairs)
            {
                bool noEntryPointsFound = entryPoints.Count == 0;

                if (pair.leftComponent is Variable<T> variable && noEntryPointsFound)
                {
                    PatternReplacer<T>.FindEntryPoints(entryPoints, substitutedFormula, variable);

                    replaceTerm = pair.rightComponent;
                }
            }

            PatternReplacer<T>.VariableReplaced(entryPoints, replaceTerm);

            return entryPoints.Count > 0 ? substitutedFormula : null;
        }

        #endregion
    }
}
