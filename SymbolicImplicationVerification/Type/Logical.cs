using System;

namespace SymbolicImplicationVerification.Type
{
    public class Logical : Type
    {
        #region Fields

        /// <summary>
        /// The singular instance of the Logical class.
        /// </summary>
        private static Logical? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private Logical() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular Logical instance.</returns>
        public static Logical Instance()
        {
            if (instance is null)
            {
                instance = new Logical();
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
