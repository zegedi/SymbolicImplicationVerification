﻿using System;
using System.Security.AccessControl;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public abstract class FunctionValue<D, T> : Term<T>
        where D : Type
        where T : Type
    {
        #region Fields

        protected Term<D> argument;

        #endregion

        #region Constructors

        public FunctionValue(Term<D> argument, T termTpye) : base(termTpye)
        {
            this.argument = argument;
        }

        #endregion

        #region Public properties

        public Term<D> Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        #endregion

        #region Public methods

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
            return obj is not null &&
                   obj is FunctionValue<D, T> other &&
                   argument.Equals(other.argument);
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
