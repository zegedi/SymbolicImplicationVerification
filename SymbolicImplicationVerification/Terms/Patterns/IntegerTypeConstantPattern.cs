using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class IntegerTypeConstantPattern : ConstantPattern<int, IntegerType>
    {
        #region Constructors

        public IntegerTypeConstantPattern(int identifier) : base(identifier,Integer.Instance()) { }

        public IntegerTypeConstantPattern(int identifier, IntegerType termType) : base(identifier, termType) { }

        public IntegerTypeConstantPattern(IntegerTypeConstantPattern constantPattern)
            : base(constantPattern.identifier, constantPattern.termType.DeepCopy()) { }

        #endregion

        #region Public static operators

        public static Addition operator +(IntegerTypeConstantPattern pattern, IntegerTypeTerm term)
        {
            return new Addition(pattern, term);
        }

        public static Subtraction operator -(IntegerTypeConstantPattern pattern, IntegerTypeTerm term)
        {
            return new Subtraction(pattern, term);
        }

        public static Multiplication operator *(IntegerTypeConstantPattern pattern, IntegerTypeTerm term)
        {
            return new Multiplication(pattern, term);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current constant pattern.
        /// </summary>
        /// <returns>The created deep copy of the constant pattern.</returns>
        public override IntegerTypeConstantPattern DeepCopy()
        {
            return new IntegerTypeConstantPattern(this);
        }

        #endregion
    }
}
