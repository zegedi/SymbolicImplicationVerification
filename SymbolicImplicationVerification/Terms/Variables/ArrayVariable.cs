using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms
{
    public class ArrayVariable<T> : Variable<T> where T : Type
    {
        #region Fields

        // protected IntegerTypeTermIndexre index;

        #endregion

        #region Constructors

        public ArrayVariable(string identifier, IntegerTypeTerm length, T termType) : base(identifier, termType)
        {
            const int firstElementIndex = 1;

            IntegerConstant firstIndex = new IntegerConstant(firstElementIndex);

            //IntegerTypeTermIndexre arrayIndexBounds = new IntegerTypeTermIndexre(firstIndex, length);
        }

        #endregion

        #region Public properties

        /*
        public Term<BoundedInteger> Length
        {
            get { return index.Index; }
        }
        */

        #endregion

        #region Public methods

        public override string Hash(HashLevel level)
        {
            return string.Format("a_{0}_{1}", identifier, identifier);
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
            return obj is not null &&
                   obj is ArrayVariable<T> other &&
                   identifier == other.identifier;
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
