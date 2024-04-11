using System;
using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms
{
    public class Summation : IntegerTypeTerm
    {
        #region Fields

        protected IntegerTypeTerm argument;

        protected IntegerTypeVariable indexVariable;

        protected BoundedIntegerType indexBounds;

        #endregion

        #region Constructors

        public Summation(Summation summation)
            : this(summation.indexVariable, summation.indexBounds, summation.argument, summation.termType) { }

        public Summation(
            IntegerTypeVariable indexVariable, BoundedIntegerType indexBounds,
            IntegerTypeTerm argument, IntegerType termType) : base(termType)
        {
            this.indexBounds   = indexBounds.DeepCopy();
            this.indexVariable = indexVariable.DeepCopy();
            this.argument      = argument.DeepCopy();
        }

        public Summation(
            IntegerTypeVariable indexVariable, IntegerTypeTerm lowerBound, IntegerTypeTerm upperBound, 
            IntegerTypeTerm argument, IntegerType termType) : base(termType)
        {
            IntegerTypeTerm lowerBoundEval = lowerBound.Evaluated();
            IntegerTypeTerm upperBoundEval = upperBound.Evaluated();

            if (lowerBoundEval is IntegerTypeConstant lowerConstant &&
                upperBoundEval is IntegerTypeConstant upperConstant)
            {
                indexBounds = new ConstantBoundedInteger(lowerConstant.Value, upperConstant.Value);
            }
            else
            {
                indexBounds = new TermBoundedInteger(lowerBoundEval, upperBoundEval);
            }

            this.indexVariable = indexVariable.DeepCopy();
            this.argument      = argument.DeepCopy();
        }

        #endregion

        #region Public properties

        public IntegerTypeTerm Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        public IntegerTypeVariable IndexVariable
        {
            get { return indexVariable; }
            set { indexVariable = value; }
        }

        public BoundedIntegerType IndexBounds
        {
            get { return indexBounds; }
            set { indexBounds = value; }
        }

        #endregion

        #region Public methods

        public override Summation DeepCopy()
        {
            return new Summation(this);
        }

        public override IntegerTypeTerm Evaluated()
        {
            if (indexBounds.IsEmpty)
            {
                return new IntegerConstant(0);
            }

            IntegerTypeTerm argumentEval = argument.Evaluated();

            return new Summation(
                indexVariable, indexBounds.LowerBound, indexBounds.UpperBound, 
                argumentEval, termType.DeepCopy());
        }

        public override string Hash(HashLevel level)
        {
            return ToString();
        }

        public override string ToString()
        {
            return string.Format("\\sum_{{0}={1}}^{{2}} {4}",
                indexVariable, indexBounds.LowerBound, indexBounds.UpperBound, argument);
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
            bool result = false;

            if (obj is Summation other)
            {
                IntegerTypeTerm otherArgumentReplaced = PatternReplacer<IntegerType>.VariableReplaced(
                    other.argument.DeepCopy(), other.indexVariable.DeepCopy(), indexVariable.DeepCopy());

                IntegerTypeTerm argumentEval      = argument.Evaluated();
                IntegerTypeTerm otherArgumentEval = otherArgumentReplaced.Evaluated();

                bool summationBoundsEqual    = indexBounds.Equals(other.indexBounds);
                bool summationArgumentsEqual = argumentEval.Equals(otherArgumentEval);

                result = summationBoundsEqual && summationArgumentsEqual;
            }

            return result;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
