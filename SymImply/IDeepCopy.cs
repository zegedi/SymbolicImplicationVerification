
namespace SymImply
{
    public interface IDeepCopy<T>
    {
        /// <summary>
        /// Creates a deep copy of the current object.
        /// </summary>
        /// <returns>The created deep copy of the object.</returns>
        public abstract T DeepCopy();
    }
}
