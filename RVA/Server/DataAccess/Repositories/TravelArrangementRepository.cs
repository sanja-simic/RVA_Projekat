using TravelSystem.Models.Entities;
using TravelSystem.Server.DataAccess.Interfaces;

namespace TravelSystem.Server.DataAccess.Repositories
{
    public class TravelArrangementRepository : BaseRepository<TravelArrangement>, ITravelArrangementRepository
    {
        public TravelArrangementRepository() : base()
        {
        }

        // Additional specific methods for TravelArrangement can be added here
    }
}
