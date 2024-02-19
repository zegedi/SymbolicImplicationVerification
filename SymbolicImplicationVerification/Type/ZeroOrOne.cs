using System;

namespace SymbolicImplicationVerification.Type
{
    public class ZeroOrOne : IntegerType
    {
        #region Fields

        /// <summary>
        /// The singular instance of the ZeroOrOne class.
        /// </summary>
        private static ZeroOrOne? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private ZeroOrOne() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular ZeroOrOne instance.</returns>
        public static ZeroOrOne Instance()
        {
            if (instance is null)
            {
                instance = new ZeroOrOne();
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
