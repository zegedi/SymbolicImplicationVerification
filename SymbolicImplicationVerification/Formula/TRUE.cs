
namespace SymbolicImplicationVerification.Formula
{
    public class TRUE : Formula
    {
        #region Fields

        /// <summary>
        /// The singular instance of the TRUE class.
        /// </summary>
        private static TRUE? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private TRUE() : base(null) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular TRUE instance.</returns>
        public static TRUE Instance()
        {
            if (instance is null)
            {
                instance = new TRUE();
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
