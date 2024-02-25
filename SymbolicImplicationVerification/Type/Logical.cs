using System;

namespace SymbolicImplicationVerification.Type
{
    public class Logical : Type, IValueValidator<bool>, ISingleton<Logical>
    {
        #region Fields

        /// <summary>
        /// The singular instance of the <see cref="Logical"/> class.
        /// </summary>
        private static Logical? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private Logical() : base() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular <see cref="Logical"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="Logical"/> instance.</returns>
        public static Logical Instance()
        {
            if (instance is null)
            {
                instance = new Logical();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular <see cref="Logical"/> instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Determines wheter the given <see cref="bool"/> value is out of range for the <see cref="Logical"/> type.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to validate.</param>
        /// <returns><see langword="false"/> - since all <see cref="bool"/> values are in range.</returns>
        public static bool IsValueOutOfRange(bool value)
        {
            return false;
        }

        /// <summary>
        /// Determines wheter the given <see cref="bool"/> value is valid for the <see cref="Logical"/> type.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to validate.</param>
        /// <returns><see langword="true"/> - since all <see cref="bool"/> values are valid.</returns>
        public static bool IsValueValid(bool value)
        {
            return true;
        }

        #endregion
    }
}
