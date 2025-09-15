namespace TravelSystem.Models.Interfaces
{
    public interface IValidatable
    {
        bool IsValid();
        string GetValidationErrors();
    }
}
