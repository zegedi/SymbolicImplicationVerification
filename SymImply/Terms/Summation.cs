using SymImply.Evaluations;
using SymImply.Formulas;
using SymImply.Formulas.Relations;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations.Binary;
using SymImply.Terms.Variables;
using SymImply.Types;

namespace SymImply.Terms
{
    public class Summation : IntegerTypeTerm
    {
        #region Fields

        /// <summary>
        /// The argument of the summation.
        /// </summary>
        protected IntegerTypeTerm argument;

        /// <summary>
        /// The index variable of the summation.
        /// </summary>
        protected IntegerTypeVariable indexVariable;

        /// <summary>
        /// The index bounds of the summation.
        /// </summary>
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
            string indexVariable, IntegerTypeTerm lowerBound, IntegerTypeTerm upperBound,
            IntegerTypeTerm argument) 
            : this(new IntegerTypeVariable(indexVariable, Integer.Instance()), 
                  lowerBound, upperBound, argument, Integer.Instance()) { }
        

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

        /// <summary>
        /// Gets os sets the argument of the summation.
        /// </summary>
        public IntegerTypeTerm Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        /// <summary>
        /// Gets os sets the index variable of the summation.
        /// </summary>
        public IntegerTypeVariable IndexVariable
        {
            get { return indexVariable; }
            set { indexVariable = value; }
        }

        /// <summary>
        /// Gets os sets the index bounds of the summation.
        /// </summary>
        public BoundedIntegerType IndexBounds
        {
            get { return indexBounds; }
            set { indexBounds = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current term.
        /// </summary>
        /// <returns>The created deep copy of the term.</returns>
        public override Summation DeepCopy()
        {
            return new Summation(this);
        }

        /// <summary>
        /// Evaluate the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override IntegerTypeTerm Evaluated()
        {
            if (indexBounds.IsEmpty)
            {
                return new IntegerTypeConstant(0);
            }

            IntegerTypeTerm argumentEval = argument.Evaluated();

            return new Summation(
                indexVariable, indexBounds.LowerBound, indexBounds.UpperBound, 
                argumentEval, termType.DeepCopy());
        }

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            return ToString();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return @$"\summation{{{indexVariable}}}{{{indexBounds.LowerBound}}}{{{indexBounds.UpperBound}}}{{{argument}}}";
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

        /// <summary>
        /// Calculates the addition with the given term.
        /// </summary>
        /// <param name="other">The other component of the addition.</param>
        /// <returns>The result of the addition.</returns>
        public IntegerTypeTerm AdditionWith(IntegerTypeTerm other)
        {
            IntegerTypeTerm? matchedIndexVariable =
                PatternReplacer<IntegerType>.MatchVariable(argument, indexVariable, other);

            if (matchedIndexVariable is not null)
            {
                IntegerTypeConstant one = new IntegerTypeConstant(1);

                Formula decreaseLowerBound = new IntegerTypeEqual(
                    one + matchedIndexVariable.DeepCopy(), indexBounds.LowerBound.DeepCopy()).CompletelyEvaluated();

                Formula increaseUpperBound = new IntegerTypeEqual(
                    matchedIndexVariable.DeepCopy(), one + indexBounds.UpperBound.DeepCopy()).CompletelyEvaluated();

                if (decreaseLowerBound is TRUE || increaseUpperBound is TRUE)
                {
                    bool decrease = decreaseLowerBound is TRUE;

                    TermBoundedInteger newBounds = new TermBoundedInteger(
                        decrease ? matchedIndexVariable.DeepCopy() : indexBounds.LowerBound.DeepCopy(),
                        decrease ? indexBounds.UpperBound.DeepCopy() : matchedIndexVariable.DeepCopy()
                    );

                    Summation result = DeepCopy();
                    result.indexBounds = newBounds;

                    return result;
                }
            }

            return new Addition(DeepCopy(), other.DeepCopy());
        }

        #endregion
    }
}
