using SymbolicImplicationVerification.Formulas;
using System;

namespace SymbolicImplicationVerification.Programs
{
    public abstract class Program : IDeepCopy<Program>
    {
        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current program.
        /// </summary>
        /// <returns>The created deep copy of the program.</returns>
        public abstract Program DeepCopy();

        #endregion
    }
}
