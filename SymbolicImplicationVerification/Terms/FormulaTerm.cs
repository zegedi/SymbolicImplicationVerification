using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Terms
{
    public class FormulaTerm : LogicalTerm
    {
        #region Fields

        private Formula formula;

        #endregion

        #region Constructors

        public FormulaTerm(FormulaTerm formulaTerm) : this(formulaTerm.formula.DeepCopy()) { }

        public FormulaTerm(Formula formula) : base(Logical.Instance())
        {
            this.formula = formula;
        }

        #endregion

        #region Public properties

        public Formula Formula
        {
            get { return formula; }
            set { formula = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return formula.ToString();
        }

        public override string Hash(HashLevel level)
        {
            return string.Empty;
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
            return obj is FormulaTerm other && formula.Equals(other.Formula);
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
        /// Creates a deep copy of the current formula term.
        /// </summary>
        /// <returns>The created deep copy of the formula term.</returns>
        public override FormulaTerm DeepCopy()
        {
            return new FormulaTerm(this);
        }

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override LogicalTerm Evaluated()
        {
            Formula formulaEvaluated = formula.Evaluated();

            return formulaEvaluated switch
            {
                TRUE  => new LogicalConstant(true),
                FALSE => new LogicalConstant(false),
                _     => new FormulaTerm(formulaEvaluated)
            };
        }

        #endregion
    }
}
