using SymImply.Formulas.Operations;
using SymImply.Terms;
using SymImply.Terms.Constants;

namespace SymImply.Formulas
{
    public class LogicalTermFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The argument of the formula.
        /// </summary>
        protected LogicalTerm argument;

        #endregion

        #region Constructors

        public LogicalTermFormula(LogicalTermFormula logicalTerm)
            : this(logicalTerm.identifier, logicalTerm.argument.DeepCopy()) { }

        public LogicalTermFormula(LogicalTerm argument) : this(null, argument) { }

        public LogicalTermFormula(string? identifier, LogicalTerm argument) : base(identifier)
        {
            this.argument = argument;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the argument of the formula.
        /// </summary>
        public LogicalTerm Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        #endregion

        #region Public static operators

        public static ConjunctionFormula operator &(LogicalTermFormula leftOperand, Formula rightOperand)
        {
            return new ConjunctionFormula(leftOperand, rightOperand);
        }

        public static DisjunctionFormula operator |(LogicalTermFormula leftOperand, Formula rightOperand)
        {
            return new DisjunctionFormula(leftOperand, rightOperand);
        }

        public static NegationFormula operator ~(LogicalTermFormula operand)
        {
            return new NegationFormula(operand);
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            if (argument is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.Negated();
            }

            if (argument is LogicalConstant logicalConstant)
            {
                return logicalConstant.Value? FALSE.Instance() : TRUE.Instance();
            }

            return new NegationFormula(DeepCopy());
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            if (argument is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.Evaluated();
            }

            LogicalTerm argumentumEval = argument.Evaluated();

            if (argumentumEval is LogicalConstant logicalConstant)
            {
                return logicalConstant.Value? FALSE.Instance() : TRUE.Instance();
            }

            return new LogicalTermFormula(argumentumEval);
        }

        /// <summary>
        /// Determines whether the specified formulaTerm is equivalent to the current formulaTerm.
        /// </summary>
        /// <param name="other">The formulaTerm to compare with the current formulaTerm.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            Formula thisEval  = CompletelyEvaluated();
            Formula otherEval = other.CompletelyEvaluated();

            bool useEquality = thisEval is LogicalTermFormula || otherEval is LogicalTermFormula;

            return useEquality ? thisEval.Equals(otherEval) : thisEval.Equivalent(otherEval);
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
            return obj is LogicalTermFormula other && argument.Equals(other.argument);
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
        /// Creates a deep copy of the current formulaTerm.
        /// </summary>
        /// <returns>The created deep copy of the formulaTerm.</returns>
        public override Formula DeepCopy()
        {
            return new LogicalTermFormula(this);
        }

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            if (argument is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.ToLatex();
            }

            string? result = argument.ToString();

            return result is not null ? result : string.Empty;
        }

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public override Formula ConjunctionWith(Formula other)
        {
            if (argument is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.ConjunctionWith(other);
            }

            if (Complements(other))
            {
                return FALSE.Instance();
            }

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        /// <summary>
        /// Calculate the disjunction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the disjunction.</param>
        /// <returns>The result of the disjunction.</returns>
        public override Formula DisjunctionWith(Formula other)
        {
            if (argument is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.DisjunctionWith(other);
            }

            if (Complements(other))
            {
                return TRUE.Instance();
            }

            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        #endregion
    }
}
