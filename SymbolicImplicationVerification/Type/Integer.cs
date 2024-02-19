using System;

namespace SymbolicImplicationVerification.Type
{
    public class Integer : IntegerType
    {
        #region Fields

        /// <summary>
        /// The singular instance of the Integer class.
        /// </summary>
        private static Integer? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private Integer() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular Integer instance.</returns>
        public static Integer Instance()
        {
            if (instance is null)
            {
                instance = new Integer();
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
