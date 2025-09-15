using TravelSystem.Models.Entities;

namespace TravelSystem.Models.Validators
{
    public class TravelArrangementValidator : BaseValidator<TravelArrangement>
    {
        public override bool Validate(TravelArrangement arrangement)
        {
            if (!base.Validate(arrangement))
                return false;

            // Additional business validation rules
            if (arrangement.Passengers?.Count > 10)
                return false;

            return true;
        }

        public override string GetValidationErrors(TravelArrangement arrangement)
        {
            var errors = base.GetValidationErrors(arrangement);

            if (arrangement.Passengers?.Count > 10)
                errors += "Maximum 10 passengers allowed per arrangement. ";

            return errors.Trim();
        }
    }
}
