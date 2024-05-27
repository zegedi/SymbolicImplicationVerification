using SymImply.Terms.Constants;
using System;
using System.Security.AccessControl;

namespace SymImply.Terms.FunctionValues
{
    public abstract class FunctionValue<D, T> : Term<T>
        where D : Type
        where T : Type
    {
        #region Fields

        /// <summary>
        /// The agurment of the function value.
        /// </summary>
        protected Term<D> argument;

        #endregion

        #region Constructors

        protected FunctionValue(FunctionValue<D, T> functionValue)
            : this(functionValue.argument.DeepCopy(), (T) functionValue.termType.DeepCopy()) { }

        protected FunctionValue(Term<D> argument, T termTpye) : base(termTpye)
        {
            this.argument = argument;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the argument of the function value.
        /// </summary>
        public Term<D> Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current function value.
        /// </summary>
        /// <returns>The created deep copy of the function value.</returns>
        public override abstract FunctionValue<D, T> DeepCopy();

        #endregion

        #region Public methods

        /// <summary>
        /// Gives information about the current term.
        /// </summary>
        /// <param name="level">The level of hashing.</param>
        /// <returns>The <see cref="string"/> that contains the information.</returns>
        public override string Hash(HashLevel level)
        {
            return argument.Hash(level);
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
            return obj is FunctionValue<D, T> other && argument.Equals(other.argument);
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
