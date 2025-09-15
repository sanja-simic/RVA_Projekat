namespace TravelSystem.Server.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        ITravelArrangementRepository TravelArrangements { get; }
        void SaveChanges();
        void Dispose();
    }
}
