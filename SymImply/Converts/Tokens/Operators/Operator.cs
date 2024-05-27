namespace SymImply.Converts.Tokens.Operators
{
    internal abstract class Operator : Token
    {
        #region Fields

        #endregion

        #region Constant values

        /// <summary>
        /// The precedence of the unary operators.
        /// </summary>
        protected const int unaryPrecedence = 8;

        /// <summary>
        /// The precedence of the multiplicative operators.
        /// </summary>
        protected const int multiplicativePrecedence = 7;

        /// <summary>
        /// The precedence of the additive operators.
        /// </summary>
        protected const int additivePrecedence = 6;

        /// <summary>
        /// The precedence of the relational operators.
        /// </summary>
        protected const int relationalPrecedence = 5;

        /// <summary>
        /// The precedence of the equality operators.
        /// </summary>
        protected const int equalityPrecedence = 4;

        /// <summary>
        /// The precedence of the conjunction operators.
        /// </summary>
        protected const int conjunctionPrecedence = 3;

        /// <summary>
        /// The precedence of the disjunction operators.
        /// </summary>
        protected const int disjunctionPrecedence = 2;

        /// <summary>
        /// The precedence of the implication operators.
        /// </summary>
        protected const int implicationPrecedence = 1;

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the precedence of the current operator.
        /// </summary>
        /// <returns>The precedence of the current operator.</returns>
        public abstract int Precedence();

        /// <summary>
        /// Determines whether the current operator is left associative.
        /// </summary>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the operator is left associative.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool LeftAssociative();

        /// <summary>
        /// Determines whether the current operator is right associative.
        /// </summary>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the operator is right associative.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool RightAssociative();

        #endregion
    }
}
