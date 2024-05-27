using SymImply.Terms.Constants;
using SymImply.Types;

namespace SymImply.Terms.Variables
{
    public class ArrayVariable<T> : Variable<T> where T : Type
    {
        #region Fields

        /// <summary>
        /// The index term of the variable.
        /// </summary>
        protected IntegerTypeTerm? indexTerm;

        /// <summary>
        /// The index bounds of the variable.
        /// </summary>
        protected BoundedIntegerType indexBounds;

        #endregion

        #region Constant values

        /// <summary>
        /// The index of the first element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the index term of the variable.
        /// </summary>
        public IntegerTypeTerm? IndexTerm
        {
            get { return indexTerm; }
            set { indexTerm = value; }
        }

        /// <summary>
        /// Gets or sets the index bounds of the variable.
        /// </summary>
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

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
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
                result = @$"\arrayvar{{{identifier}}}{{{indexTerm}}}";
            }

            return result;
        }

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
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
