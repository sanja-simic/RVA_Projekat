using TravelSystem.Models.Enums;

namespace TravelSystem.Models.Interfaces
{
    public interface IStateful
    {
        EntityState State { get; set; }
        void ChangeState();
        string GetCurrentStateName();
    }
}
