﻿namespace Saneej.FunBooksAndVideos.Service.Models
{
    public class ResponseWrapper<T>
    {
        public bool IsClientError { get; }
        public bool IsNotFound { get; }
        public bool IsUnAuthorized { get; }
        public bool HasError => IsClientError || IsNotFound || IsUnAuthorized;
        public T Data { get; }

        public string ErrorMessage { get; }

        public ResponseWrapper(T data)
        {
            Data = data;
        }

        private ResponseWrapper(ResponseWrapper response)
        {
            IsClientError = response.IsClientError;
            IsNotFound = response.IsNotFound;
            IsUnAuthorized = response.IsUnAuthorized;
            ErrorMessage = response.ErrorMessage;
        }

        public static implicit operator ResponseWrapper<T>(ResponseWrapper response)
        {
            return new ResponseWrapper<T>(response);
        }
    }
}
