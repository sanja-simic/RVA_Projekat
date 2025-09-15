using TravelSystem.Models.Interfaces;

namespace TravelSystem.Models.Validators
{
    public abstract class BaseValidator<T> where T : IValidatable
    {
        public virtual bool Validate(T entity)
        {
            return entity.IsValid();
        }

        public virtual string GetValidationErrors(T entity)
        {
            return entity.GetValidationErrors();
        }
    }
}
