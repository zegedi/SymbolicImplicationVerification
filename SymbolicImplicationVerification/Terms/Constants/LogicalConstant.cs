using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class LogicalConstant : Constant<bool, Logical>
    {
        #region Constructors

        public LogicalConstant(bool value) : base(value, Logical.Instance()) { }

        public LogicalConstant(LogicalConstant constant) : base(constant.value, Logical.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return value ? "\\true" : "\\false";
        }

        /// <summary>
        /// Create a deep copy of the current logical constant.
        /// </summary>
        /// <returns>The created deep copy of the logical constant.</returns>
        public override LogicalConstant DeepCopy()
        {
            return new LogicalConstant(this);
        }

        /// <summary>
        /// Evaluated the given constant, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override LogicalConstant Evaluated()
        {
            return new LogicalConstant(this);
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
            return obj is LogicalConstant other && value == other.value;
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
