using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Types;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class Formula : INegatable<Formula>, IEvaluable<Formula>, IDeepCopy<Formula>
    {
        #region Fields

        protected string? identifier;

        #endregion

        #region Constructors

        public Formula(string? identifier)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        public string? Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract Formula Negated();

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract Formula Evaluated();

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
        public abstract bool Equivalent(Formula other);

        /// <summary>
        /// Determines whether the current formula implies the specified formula.
        /// </summary>
        /// <param name="other">The consequence of the implication.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the current formula implies the consequence formula.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public bool Implies(Formula consequence)
        {
            bool implies = Equivalent(consequence);
            
            if (!implies && this is BinaryRelationFormula<IntegerType> first &&
                consequence      is BinaryRelationFormula<IntegerType> second)
            {
                Formula intersection = first.ConjunctionWith(second);

                implies = intersection.Equivalent(first);
            }

            return implies;
        }

        /// <summary>
        /// Creates a deep copy of the current formula.
        /// </summary>
        /// <returns>The created deep copy of the formula.</returns>
        public abstract Formula DeepCopy();

        #endregion

        #region Protected methods

        /// <summary>
        /// Determines whether the specified formula complements the current formula.
        /// </summary>
        /// <param name="other">The other formula.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the specified formula is equivalent to the negated current formula.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public bool Complements(Formula other)
        {
            return Equivalent(other.Negated());
        }

        #endregion
    }
}
