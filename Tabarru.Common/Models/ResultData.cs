using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tabarru.Common.Enums;

namespace Tabarru.Common.Models
{
    public class ResultData
    {
        internal ResultData()
        {

        }

        public ResultData(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResultData(bool status, string message, bool isConflict)
        {
            Status = status;
            Message = message;
            Conflict = isConflict;
        }

        public ResultData(IEnumerable<string> errors)
        {
            Errors = errors;
            Status = false;
            Code = ResponseCode.Errors;
        }

        public ResultData(IEnumerable<string> errors, bool isConflict)
        {
            Errors = errors;
            Status = false;
            Code = ResponseCode.Errors;
            Conflict = isConflict;
        }

        internal ResultData(string message, ResponseCode code = ResponseCode.Message)
        {
            Message = message;
            Code = code;
            Status = code == ResponseCode.Message;
        }

        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ResponseCode Code { get; set; }
        public bool Status { get; set; }
        [JsonIgnore]
        public bool Conflict { get; set; }
    }

    public class ResultData<T> : ResultData
    {
        internal ResultData()
        {

        }

        public ResultData(bool status, string message) : base(status, message)
        {

        }

        public ResultData(T data)
        {
            Status = true;
            Data = data;
            Code = ResponseCode.Data;
        }

        internal ResultData(string message, ResponseCode code = ResponseCode.Message)
        {
            Message = message;
            Code = code;
        }

        internal ResultData(IEnumerable<string> errors)
        {
            Errors = errors;
            Code = ResponseCode.Errors;
        }

        public T Data { get; internal set; }
    }
}
