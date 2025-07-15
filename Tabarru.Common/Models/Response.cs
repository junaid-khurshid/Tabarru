using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tabarru.Common.Enums;

namespace Tabarru.Common.Models
{
    public class Response : ObjectResult
    {
        public Response(string message)
            : base(new ResultData(message))
        {
            StatusCode = (int)HttpStatusCode.OK;
        }

        public Response(HttpStatusCode httpStatusCode)
            : base(null)
        {
            StatusCode = (int)httpStatusCode;
        }

        public Response(HttpStatusCode httpStatusCode, string message)
            : base(new ResultData(message, ((int)httpStatusCode >= 200 && (int)httpStatusCode < 300) ? ResponseCode.Message : ResponseCode.Error))
        {
            StatusCode = (int)httpStatusCode;
        }

        public Response(HttpStatusCode httpStatusCode, IEnumerable<string> errors)
            : base(new ResultData(errors))
        {
            StatusCode = (int)httpStatusCode;
        }
    }

    public class Response<T> : ObjectResult
    {
        public Response(T data)
            : base(new ResultData<T>(data))
        {
            StatusCode = (int)HttpStatusCode.OK;
        }

        public Response(HttpStatusCode httpStatusCode)
            : base(new ResultData<T>())
        {
            StatusCode = (int)httpStatusCode;
            Value = null;
        }

        public Response(HttpStatusCode httpStatusCode, string message)
            : base(new ResultData<T>(message, httpStatusCode == HttpStatusCode.BadRequest ? ResponseCode.Error : ResponseCode.Message))
        {
            StatusCode = (int)httpStatusCode;
        }
    }
}
