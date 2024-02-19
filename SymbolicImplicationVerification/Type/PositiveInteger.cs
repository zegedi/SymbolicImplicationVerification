using System;

namespace SymbolicImplicationVerification.Type
{
    public class PositiveInteger : IntegerType
    {
        #region Fields

        /// <summary>
        /// The singular instance of the PositiveInteger class.
        /// </summary>
        private static PositiveInteger? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private PositiveInteger() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular PositiveInteger instance.</returns>
        public static PositiveInteger Instance()
        {
            if (instance is null)
            {
                instance = new PositiveInteger();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        #endregion
    }
}
