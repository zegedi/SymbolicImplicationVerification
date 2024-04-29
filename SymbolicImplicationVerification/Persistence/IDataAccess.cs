namespace SymbolicImplicationVerification.Persistence
{
    public interface IDataAccess
    {
        /// <summary>
        /// Loads the given object.
        /// </summary>
        /// <param name="path">The path of the source file.</param>
        /// <returns>The loaded object.</returns>
        Task LoadAsync(string path);

        /// <summary>
        /// Saves the given object.
        /// </summary>
        /// <param name="path">The path of the destination file.</param>
        /// <param name="obj">The object to save.</param>
        Task SaveAsync(string path);
    }
}
