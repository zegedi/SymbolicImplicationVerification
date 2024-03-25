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
        /// Create a deep copy of the current logical constant.
        /// </summary>
        /// <returns>The created deep copy of the logical constant.</returns>
        public override LogicalConstant DeepCopy()
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
            return obj is not null &&
                   obj is LogicalConstant other &&
                   value == other.value;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /*
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
            return obj is not null &&
                   obj is LogicalConstant constant &&
                   value == constant.value;
        }
        */

        #endregion
    }
}
