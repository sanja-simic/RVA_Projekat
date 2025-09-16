namespace TravelSystem.Models.Entities
{
    public class Destination : BaseEntity
    {
        public string TownName { get; set; }
        public string CountryName { get; set; }
        public double StayPriceByDay { get; set; }

        public Destination() : base() { }

        public Destination(string id, string townName, string countryName, double stayPriceByDay) 
            : base(id)
        {
            TownName = townName;
            CountryName = countryName;
            StayPriceByDay = stayPriceByDay;
        }

        public override bool IsValid()
        {
            return base.IsValid() && 
                   !string.IsNullOrEmpty(TownName) && 
                   !string.IsNullOrEmpty(CountryName) && 
                   StayPriceByDay > 0;
        }

        public override string GetValidationErrors()
        {
            var errors = base.GetValidationErrors();
            
            if (string.IsNullOrEmpty(TownName))
                errors += "Town name cannot be null or empty. ";
                
            if (string.IsNullOrEmpty(CountryName))
                errors += "Country name cannot be null or empty. ";
                
            if (StayPriceByDay <= 0)
                errors += "Stay price by day must be greater than 0. ";
                
            return errors.Trim();
        }
    }
}
