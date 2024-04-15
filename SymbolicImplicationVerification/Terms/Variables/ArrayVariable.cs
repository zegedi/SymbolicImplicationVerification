using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Variables
{
    public class ArrayVariable<T> : Variable<T> where T : Type
    {
        #region Fields

        protected IntegerTypeTerm? indexTerm;

        protected BoundedIntegerType indexBounds;

        #endregion

        #region Constant values

        const int firstElementIndex = 1;

        #endregion

        #region Constructors

        public ArrayVariable(ArrayVariable<T> variable) : this(
            variable.identifier, 
            variable.indexBounds.UpperBound, 
            variable.indexTerm, 
            (T) variable.termType.DeepCopy()) { }

        public ArrayVariable(string identifier, IntegerTypeTerm length, T termType)
            : this(identifier, length, null, termType) { }

        public ArrayVariable(string identifier, IntegerTypeTerm length, IntegerTypeTerm? indexTerm, T termType)
            : base(identifier, termType)
        {
            IntegerTypeTerm lengthEval = length.Evaluated();

            indexBounds = lengthEval is IntegerTypeConstant constantLength ?
                          new ConstantBoundedInteger(firstElementIndex, constantLength.Value) :
                          new TermBoundedInteger(firstElementIndex, lengthEval);

            this.indexTerm = indexTerm?.DeepCopy();
        }

        #endregion

        #region Public properties

        public IntegerTypeTerm? IndexTerm
        {
            get { return indexTerm; }
            set { indexTerm = value; }
        }

        protected BoundedIntegerType IndexBounds
        {
            get { return indexBounds; }
            set { indexBounds = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current variable.
        /// </summary>
        /// <returns>The created deep copy of the variable.</returns>
        public override ArrayVariable<T> DeepCopy()
        {
            return new ArrayVariable<T>(this);
        }

        public override string Hash(HashLevel level)
        {
            return string.Format("a_{0}_{1}", identifier, indexTerm);
        }

        /// <summary>
        /// Evaluated the given variable, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override ArrayVariable<T> Evaluated()
        {
            IntegerTypeTerm? simplifiedIndexTerm = indexTerm?.Evaluated();

            return new ArrayVariable<T>(
                identifier, indexBounds.UpperBound, simplifiedIndexTerm, (T) termType.DeepCopy());
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = identifier;

            if (indexTerm is not null)
            {
                result = string.Format("{0}[{1}]", identifier, indexTerm);
            }

            return result;
        }

        public override bool Matches(object? obj)
        {
            bool result = false;

            if (obj is ArrayVariable<T> other)
            {
                result = identifier == other.identifier;

                if (indexTerm is not null)
                {
                    result &= indexTerm.Matches(other.indexTerm);
                }
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
            bool result = false;

            if (obj is ArrayVariable<T> other)
            {
                result  = identifier == other.identifier;
                result &= (indexTerm is null && other.indexTerm is null) ||
                          (indexTerm is not null && indexTerm.Equals(other.indexTerm));
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
