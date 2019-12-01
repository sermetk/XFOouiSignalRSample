namespace OouiSignalRSample.Core.Service
{
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public ServiceResult()
        {

        }

        public ServiceResult(string message, bool success) : base(message, success)
        {

        }

        public ServiceResult(string message, bool success, T data) : base(message, success)
        {
            Data = data;
        }
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public StatusCode Status { get; set; }
        public ServiceResult()
        {

        }

        public ServiceResult(string message, bool success)
        {
            Success = success;
            Message = message;
        }
        public enum StatusCode
        {
            Success = 200,
            SystemEx = 300,
            TokenExpire = 401,
            NotImplemented = 0
        }
    }
}
