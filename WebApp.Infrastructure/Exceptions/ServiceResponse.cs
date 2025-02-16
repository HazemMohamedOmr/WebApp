using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Interfaces;

namespace WebApp.Infrastructure.Exceptions
{
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<Object>? Errors { get; set; }

        public static ServiceResponse<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new ServiceResponse<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ServiceResponse<T> Fail(string message, int statusCode = 400, IEnumerable<Object>? errors = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors
            };
        }
    }
}