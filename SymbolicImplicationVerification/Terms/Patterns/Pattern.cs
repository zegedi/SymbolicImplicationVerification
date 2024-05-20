using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public abstract class Pattern<T> : Term<T> where T : Type
    {
        #region Fields

        /// <summary>
        /// The indentifier or the pattern.
        /// </summary>
        protected readonly int identifier;

        #endregion

        #region Constructors

        protected Pattern(int identifier, T termType) : base(termType) 
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the indentifier of the pattern.
        /// </summary>
        public int Identifier
        {
            get { return identifier; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current pattern.
        /// </summary>
        /// <returns>The created deep copy of the pattern.</returns>
        public override abstract Pattern<T> DeepCopy();

        /// <summary>
        /// Evaluated the given pattern, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override abstract Pattern<T> Evaluated();

        #endregion

        #region Public methods

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
            return obj is Pattern<T> other && identifier == other.identifier;
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
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            return Convert.ToString(identifier);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <param name="term">The expected runtime generic type of the <see cref="object"/>.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        protected bool Matches(object? obj, System.Type term)
        {
            if (obj is null)
            {
                return false;
            }

            // Getting the runtime type of the parameter.
            System.Type? runtimeType = obj.GetType();

            while (runtimeType is not null)
            {
                // Checking if the obj parameter is a subclass of the term.
                if (runtimeType.IsGenericType &&
                    runtimeType.GetGenericTypeDefinition() == term)
                {
                    PropertyInfo? termTypeProperty = runtimeType.GetProperty("TermType");

                    return termTypeProperty is not null &&
                           termTypeProperty.GetValue(obj) is T;
                }

                // Getting the next base type.
                runtimeType = runtimeType.BaseType;
            }

            // The obj parameter does not match the pattern.
            return false;
        }

        #endregion
    }
}
