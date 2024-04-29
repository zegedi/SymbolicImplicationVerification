using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Converts.Tokens.Functions
{
    internal abstract class FunctionCallEnd : Token
    {
        #region Public abstract methods

        /// <summary>
        /// Determines whether the current function call end closes the open function call.
        /// </summary>
        /// <param name="start">The given start function call.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the end closes the open function call.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool Closes(FunctionCallStart start);

        #endregion
    }
}
