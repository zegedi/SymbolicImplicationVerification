global using Type = SymbolicImplicationVerification.Types.Type;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Types
{
    public abstract class Type : IValueValidator<object?>, IDeepCopy<Type>
    {
        #region Constructors

        public Type() { }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public abstract Type DeepCopy();

        /// <summary>
        /// Determines whether the assigned type is compatible with the given type.
        /// </summary>
        /// <param name="assignedType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the two types are compatible.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool TypeCompatible(Type assignedType);

        /// <summary>
        /// Determines whether the assigned type is directly assignable to the given type.
        /// </summary>
        /// <param name="assignedType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the assigned type is directly assignable.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool TypeAssignable(Type assignedType);

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
        public abstract bool IsValueOutOfRange(object? value);

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
        public abstract bool IsValueValid(object? value);

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public abstract Formula TypeConstraintOn(TypeTerm term);

        /// <summary>
        /// Calculates the intersection of the two types.
        /// </summary>
        /// <param name="other">The other argument of the intersection.</param>
        /// <returns>The intersection of the types.</returns>
        public abstract Type? Intersection(Type other);

        /// <summary>
        /// Calculates the union of the two types.
        /// </summary>
        /// <param name="other">The other argument of the union.</param>
        /// <returns>The union of the types.</returns>
        public abstract Type? Union(Type other);

        #endregion
    }
}
