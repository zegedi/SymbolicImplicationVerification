global using Type = SymbolicImplicationVerification.Types.Type;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Types
{
    public abstract class Type : IValueValidator<object?>, IDeepCopy<Type>
    {
        #region Constructors

        public Type() { }

        #endregion

        #region Public static operators

        public static bool operator ==(Type first, Type second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Type first, Type second)
        {
            return !first.Equals(second);
        }

        #endregion

        #region Public abstract methods

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
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


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
