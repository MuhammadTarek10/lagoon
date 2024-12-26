namespace Lagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        void Save();
    }
}