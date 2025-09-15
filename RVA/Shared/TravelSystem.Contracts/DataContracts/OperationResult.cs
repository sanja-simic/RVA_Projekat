using System.Runtime.Serialization;

namespace TravelSystem.Contracts.DataContracts
{
    [DataContract]
    public class OperationResult
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public OperationResult()
        {
            Timestamp = System.DateTime.Now;
        }

        public static OperationResult Success(string message = "Operation completed successfully")
        {
            return new OperationResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static OperationResult Error(string message, string errorCode = null)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
