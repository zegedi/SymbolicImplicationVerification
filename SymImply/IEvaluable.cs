namespace SymImply
{
    public interface IEvaluable<T>
    {
        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public T Evaluated();
    }
}
