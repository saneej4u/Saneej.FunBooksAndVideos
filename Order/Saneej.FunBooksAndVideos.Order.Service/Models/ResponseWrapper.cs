namespace Saneej.FunBooksAndVideos.Service.Models
{
    public class ResponseWrapper
    {
        public bool IsClientError { get; }
        public bool IsNotFound { get; }
        public bool IsUnAuthorized { get; }

        public bool HasError => IsClientError || IsNotFound || IsUnAuthorized;

        public string ErrorMessage { get; }

        protected ResponseWrapper()
        {
        }

        protected ResponseWrapper(bool isClientError, bool isNotFound, bool isUnAuthorized, string erroMessage)
        {
            IsClientError = isClientError;
            IsNotFound = isNotFound;
            IsUnAuthorized = isUnAuthorized;
            ErrorMessage = erroMessage;
        }

        public static ResponseWrapper CreateSuccess()
        {
            return new ResponseWrapper();
        }

        public static ResponseWrapper<T> CreateSuccess<T>(T data)
        {
            return new ResponseWrapper<T>(data);
        }

        public static ResponseWrapper CreateNotFoundError(string errorMessage = null)
        {
            return new ResponseWrapper(false, true, false, errorMessage);
        }

        public static ResponseWrapper CreateClientError(string errorMessage = null)
        {
            return new ResponseWrapper(true, false, false, errorMessage);
        }

        public static ResponseWrapper CreateUnAuthorized(string errorMessage = null)
        {
            return new ResponseWrapper(false, false, true, errorMessage);
        }
    }
}
