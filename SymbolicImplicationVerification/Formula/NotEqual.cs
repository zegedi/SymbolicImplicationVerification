﻿using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Formula
{
    public class NotEqual<T> : BinaryRelationFormula<T> where T : Type.Type
    {
        #region Constructors

        public NotEqual(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public NotEqual(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
