using System.Runtime.Serialization;

namespace TravelSystem.Contracts.Faults
{
    [DataContract]
    public class ServiceFault
    {
        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public ServiceFault()
        {
            Timestamp = System.DateTime.Now;
        }

        public ServiceFault(string operation, string errorMessage, string errorCode = null) : this()
        {
            Operation = operation;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}
