namespace TravelSystem.Models.DTOs
{
    public class PassengerDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public int LuggageWeight { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
