using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Common
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public bool Progress { get; set; }

        public static ApiResponse<T> CreateSuccessMessage(T data, string message)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Progress = true
            };
        }

        public static ApiResponse<T> CreateFailureMessage(string message)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                Progress = false
            };
        }


    }
}
