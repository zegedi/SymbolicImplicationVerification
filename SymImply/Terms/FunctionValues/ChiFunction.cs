using SymImply.Terms.Constants;
using SymImply.Types;
using System;

namespace SymImply.Terms.FunctionValues
{
    public class ChiFunction : FunctionValue<Logical, IntegerType>
    {
        #region Constructors

        public ChiFunction(LogicalTerm argument) : base(argument, ZeroOrOne.Instance()) { }

        public ChiFunction(ChiFunction chiFunction) : this(chiFunction.argument.DeepCopy()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return @$"\chifunc{{{argument}}}";
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
            return obj is ChiFunction other && argument.Equals(other.argument);
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
        /// Create a deep copy of the current chi function.
        /// </summary>
        /// <returns>The created deep copy of the chi function.</returns>
        public override ChiFunction DeepCopy()
        {
            return new ChiFunction(this);
        }

        /// <summary>
        /// Evaluated the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override IntegerTypeTerm Evaluated() => argument.Evaluated() switch
        {
            LogicalConstant constant => new IntegerTypeConstant(constant.Value ? 1 : 0),
            LogicalTerm     argument => new ChiFunction(argument)
        };

        #endregion
    }
}
