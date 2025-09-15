using System.Runtime.Serialization;

namespace TravelSystem.Contracts.DataContracts
{
    [DataContract]
    public class ServiceRequest<T>
    {
        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public ServiceRequest()
        {
            RequestId = System.Guid.NewGuid().ToString();
            Timestamp = System.DateTime.Now;
        }

        public ServiceRequest(T data) : this()
        {
            Data = data;
        }
    }
}
