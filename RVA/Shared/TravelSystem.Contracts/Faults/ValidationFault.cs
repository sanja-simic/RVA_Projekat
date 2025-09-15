using System.Runtime.Serialization;

namespace TravelSystem.Contracts.Faults
{
    [DataContract]
    public class ValidationFault
    {
        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public string EntityId { get; set; }

        [DataMember]
        public string ValidationErrors { get; set; }

        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public ValidationFault()
        {
            Timestamp = System.DateTime.Now;
        }

        public ValidationFault(string entityType, string entityId, string validationErrors) : this()
        {
            EntityType = entityType;
            EntityId = entityId;
            ValidationErrors = validationErrors;
        }
    }
}
