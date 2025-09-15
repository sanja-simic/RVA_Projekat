using System.Runtime.Serialization;

namespace TravelSystem.Contracts.DataContracts
{
    [DataContract]
    public class ServiceResponse<T>
    {
        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public ServiceResponse()
        {
            Timestamp = System.DateTime.Now;
            IsSuccess = true;
        }

        public ServiceResponse(T data) : this()
        {
            Data = data;
        }

        public ServiceResponse(string errorMessage, string requestId = null) : this()
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            RequestId = requestId;
        }

        public static ServiceResponse<T> Success(T data, string requestId = null)
        {
            return new ServiceResponse<T>(data) { RequestId = requestId };
        }

        public static ServiceResponse<T> Error(string errorMessage, string requestId = null)
        {
            return new ServiceResponse<T>(errorMessage, requestId);
        }
    }
}
