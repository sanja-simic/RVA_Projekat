namespace TravelSystem.Models.Entities
{
    public class Passenger : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public int LuggageWeight { get; set; }

        public Passenger() : base() { }

        public Passenger(string id, string firstName, string lastName, string passportNumber, int luggageWeight) 
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            PassportNumber = passportNumber;
            LuggageWeight = luggageWeight;
        }

        public override bool IsValid()
        {
            return base.IsValid() && 
                   !string.IsNullOrEmpty(FirstName) && 
                   !string.IsNullOrEmpty(LastName) && 
                   !string.IsNullOrEmpty(PassportNumber) &&
                   LuggageWeight >= 0;
        }

        public override string GetValidationErrors()
        {
            var errors = base.GetValidationErrors();
            
            if (string.IsNullOrEmpty(FirstName))
                errors += "First name cannot be null or empty. ";
                
            if (string.IsNullOrEmpty(LastName))
                errors += "Last name cannot be null or empty. ";
                
            if (string.IsNullOrEmpty(PassportNumber))
                errors += "Passport number cannot be null or empty. ";
                
            if (LuggageWeight < 0)
                errors += "Luggage weight cannot be negative. ";
                
            return errors.Trim();
        }
    }
}
