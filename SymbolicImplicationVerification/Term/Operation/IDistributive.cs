
namespace SymbolicImplicationVerification.Term.Operation
{
    public interface IDistributive<T>
    {
        public T Distribute();

        public T Factor();
    }
}
