namespace SymbolicImplicationVerification.Formulas
{
    public interface IEvaluable<T>
    {
        /*
        /// <summary>
        /// Evalue the given expression. It can change the original expression.
        /// </summary>
        public void Evaluate();
        */

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public T Evaluated();
    }
}
