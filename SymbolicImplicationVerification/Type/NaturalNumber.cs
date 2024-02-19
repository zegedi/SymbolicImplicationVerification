using System;

namespace SymbolicImplicationVerification.Type
{
    public class NaturalNumber : IntegerType
    {
        #region Fields

        /// <summary>
        /// The singular instance of the NaturalNumber class.
        /// </summary>
        private static NaturalNumber? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private NaturalNumber() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular NaturalNumber instance.</returns>
        public static NaturalNumber Instance()
        {
            if (instance is null)
            {
                instance = new NaturalNumber();
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
