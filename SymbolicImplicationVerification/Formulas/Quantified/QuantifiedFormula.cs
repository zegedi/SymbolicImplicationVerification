using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Quantified
{
    public abstract class QuantifiedFormula<T> : Formula where T : Type
    {
        #region Fields

        protected Variable<T> quantifiedVariable;

        protected Formula statement;

        #endregion

        #region Constructors

        public QuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) : this(null, quantifiedVariable, statement) { }

        public QuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement) : base(identifier)
        {
            this.quantifiedVariable = quantifiedVariable;
            this.statement = statement;
        }

        #endregion

        #region Public properties

        public Variable<T> QuantifiedVariable
        {
            get { return quantifiedVariable; }
            set { quantifiedVariable = value; }
        }

        public Formula Statement
        {
            get { return statement; }
            set { statement = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current quantified program.
        /// </summary>
        /// <returns>The created deep copy of the quantified program.</returns>
        public override abstract QuantifiedFormula<T> DeepCopy();

        #endregion

        #region Public methods

        public bool StatementsEquivalent(QuantifiedFormula<T> other)
        {
            Formula otherStatement = other.statement.DeepCopy();

            if (!quantifiedVariable.Equals(other.quantifiedVariable))
            {
                PatternReplacer<T>.VariableReplaced(
                    otherStatement, other.quantifiedVariable.DeepCopy(), quantifiedVariable.DeepCopy());
            }

            return statement.Equivalent(otherStatement);
        }

        public Formula ConjunctionWith(QuantifiedFormula<IntegerType> quantified, Formula statement)
        {
            IntegerTypeTerm? variableReplaceTerm 
                = PatternReplacer<IntegerType>.QuantifiedVariableReplaced(quantified, statement);

            if (variableReplaceTerm is not null &&
                quantified.quantifiedVariable.TermType is BoundedIntegerType bounded)
            {
                IntegerConstant one = new IntegerConstant(1);

                Formula decreaseLowerBound = new IntegerTypeEqual(
                    one + variableReplaceTerm.DeepCopy(), bounded.LowerBound.DeepCopy()).Evaluated();

                Formula increaseUpperBound = new IntegerTypeEqual(
                    variableReplaceTerm.DeepCopy(), one + bounded.UpperBound.DeepCopy()).Evaluated();

                if (decreaseLowerBound is TRUE || increaseUpperBound is TRUE)
                {
                    bool decrease = decreaseLowerBound is TRUE;

                    TermBoundedInteger newBounds = new TermBoundedInteger(
                        decrease ? variableReplaceTerm.DeepCopy() : bounded.LowerBound.DeepCopy(),
                        decrease ? bounded.UpperBound.DeepCopy() : variableReplaceTerm.DeepCopy()
                    );

                    QuantifiedFormula<IntegerType> result = quantified.DeepCopy();
                    result.quantifiedVariable.TermType = newBounds;

                    return result;
                }
            }

            return new ConjunctionFormula(quantified, statement);
        }

        #endregion
    }
}
