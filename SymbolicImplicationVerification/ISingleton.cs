using SymbolicImplicationVerification.Types;
using System;

namespace SymbolicImplicationVerification
{
    public interface ISingleton<T>
    {
        /// <summary>
        /// Factory method for the singular <see cref="T"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="T"/> instance.</returns>
        public abstract static T Instance();

        /// <summary>
        /// Destroy method for the singular <see cref="T"/> instance.
        /// </summary>
        public abstract static void Destroy();
    }
}
