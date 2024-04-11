using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms.Constants;
using System;

namespace SymbolicImplicationVerification.Formulas
{
    public class FALSE : Formula
    {
        #region Fields

        /// <summary>
        /// The singular instance of the FALSE class.
        /// </summary>
        private static FALSE? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private FALSE() : base(null) { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular FALSE instance.</returns>
        public static FALSE Instance()
        {
            if (instance is null)
            {
                instance = new FALSE();
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

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return "\\FALSE";
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            return FALSE.Instance();
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return TRUE.Instance();
        }

        /// <summary>
        /// Determines whether the specified formula is equivalent to the current formula.
        /// </summary>
        /// <param name="other">The formula to compare with the current formula.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            return other.Evaluated() is FALSE;
        }

        public override Formula ConjunctionWith(Formula other)
        {
            return other is NotEvaluable ? NotEvaluable.Instance() : FALSE.Instance();
        }

        public override Formula DisjunctionWith(Formula other)
        {
            return other is NotEvaluable ? NotEvaluable.Instance() : other.DeepCopy();
        }

        /// <summary>
        /// Creates a deep copy of the current formula.
        /// </summary>
        /// <returns>The created deep copy of the formula.</returns>
        public override FALSE DeepCopy()
        {
            return FALSE.Instance();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is FALSE;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
