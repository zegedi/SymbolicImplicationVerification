

namespace SymImply
{
    public interface IValueValidator<T>
    {
        /// <summary>
        /// Determines whether the given <see cref="T"/> value is out of range for the given type.
        /// </summary>
        /// <param name="value">The <see cref="T"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is out of range.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool IsValueOutOfRange(T value);

        /// <summary>
        /// Determines whether the given <see cref="T"/> value is valid for the given type.
        /// </summary>
        /// <param name="value">The <see cref="T"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is valid.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool IsValueValid(T value);
    }
}
