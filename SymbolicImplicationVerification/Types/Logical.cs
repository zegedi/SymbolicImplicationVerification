using SymbolicImplicationVerification.Formulas;
using System;

namespace SymbolicImplicationVerification.Types
{
    public class Logical : Type, IValueValidator<bool>, ISingleton<Logical>
    {
        #region Fields

        /// <summary>
        /// The singular instance of the <see cref="Logical"/> class.
        /// </summary>
        private static Logical? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private Logical() : base() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular <see cref="Logical"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="Logical"/> instance.</returns>
        public static Logical Instance()
        {
            if (instance is null)
            {
                instance = new Logical();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular <see cref="Logical"/> instance.
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
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override Logical DeepCopy()
        {
            return Logical.Instance();
        }

        /// <summary>
        /// Determines whether the assigned type is compatible with the given type.
        /// </summary>
        /// <param name="assignType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the assign type is an <see cref="Logical"/>.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool TypeCompatible(Type assignType)
        {
            return assignType is Logical;
        }

        /// <summary>
        /// Determines whether the assigned type is directly assignable to the given type.
        /// </summary>
        /// <param name="assignType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the assigned type is directly assignable.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool TypeAssignable(Type assignType)
        {
            return TypeCompatible(assignType);
        }

        /// <summary>
        /// Determines whether the given <see cref="object"/>? value is out of range for the given type.
        /// </summary>
        /// <param name="value">The <see cref="object"/>? value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is out of range.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueOutOfRange(object? value)
        {
            return value is null ||
                   value is not bool logicalValue ||
                   IsValueOutOfRange(logicalValue);
        }

        /// <summary>
        /// Determines whether the given <see cref="object"/>? value is valid for the given type.
        /// </summary>
        /// <param name="value">The <see cref="object"/>? value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is valid.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueValid(object? value)
        {
            return value is not null &&
                   value is bool logicalValue &&
                   IsValueValid(logicalValue);
        }

        /// <summary>
        /// Determines wheter the given <see cref="bool"/> value is out of range
        /// for the <see cref="Logical"/> type.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to validate.</param>
        /// <returns><see langword="false"/> - since all <see cref="bool"/> values are in range.</returns>
        public bool IsValueOutOfRange(bool value)
        {
            return false;
        }

        /// <summary>
        /// Determines wheter the given <see cref="bool"/> value is valid for the <see cref="Logical"/> type.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to validate.</param>
        /// <returns><see langword="true"/> - since all <see cref="bool"/> values are valid.</returns>
        public bool IsValueValid(bool value)
        {
            return true;
        }

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(TypeTerm term)
        {
            bool isLogicalTerm = term.TermType is Logical;

            return isLogicalTerm ? TRUE.Instance() : FALSE.Instance();
        }

        #endregion
    }
}
