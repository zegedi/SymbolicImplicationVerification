using System;

namespace SymbolicImplicationVerification.Formulas
{
    public class FALSE : Formula
    {
        #region Fields

        /// <summary>
        /// The singular instance of the FALSE class.
        /// </summary>
        private static FALSE? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private FALSE() : base(null) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular FALSE instance.</returns>
        public static FALSE Instance()
        {
            if (instance is null)
            {
                instance = new FALSE();
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
