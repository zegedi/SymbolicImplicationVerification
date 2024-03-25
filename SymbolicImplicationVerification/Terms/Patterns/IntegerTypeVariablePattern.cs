﻿using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class IntegerTypeVariablePattern : VariablePattern<IntegerType>
    {
        #region Constructors

        public IntegerTypeVariablePattern(int identifier) : base(identifier, Integer.Instance()) { }

        public IntegerTypeVariablePattern(int identifier, IntegerType termType) : base(identifier, termType) { }

        public IntegerTypeVariablePattern(IntegerTypeVariablePattern constantPattern)
            : base(constantPattern.identifier, constantPattern.termType.DeepCopy()) { }

        #endregion

        #region Public static operators

        public static Addition operator +(IntegerTypeVariablePattern pattern, IntegerTypeTerm term)
        {
            return new Addition(pattern, term);
        }

        public static Subtraction operator -(IntegerTypeVariablePattern pattern, IntegerTypeTerm term)
        {
            return new Subtraction(pattern, term);
        }

        public static Multiplication operator *(IntegerTypeVariablePattern pattern, IntegerTypeTerm term)
        {
            return new Multiplication(pattern, term);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current constant pattern.
        /// </summary>
        /// <returns>The created deep copy of the constant pattern.</returns>
        public override IntegerTypeVariablePattern DeepCopy()
        {
            return new IntegerTypeVariablePattern(this);
        }

        #endregion
    }
}
