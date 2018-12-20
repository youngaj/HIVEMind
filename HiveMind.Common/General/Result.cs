using HiveMind.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiveMind.Common.General
{
    public class Result : IResult
    {

        private List<String> messages;
        /// <summary>
        /// Gets or set result message
        /// </summary>
        public string Message
        {
            get
            {
                StringBuilder msgOut = new StringBuilder();
                foreach (var msg in messages)
                {
                    msgOut.Append(msg);
                }
                return msgOut.ToString();
            }
            set
            {
                ClearMessages();
                AddMessage(value);
            }
        }

        public ResultType Type { get; set; }

        public Object Obj { get; set; }

        public bool IsSuccessful
        {
            get
            {
                bool result;
                if (Type == ResultType.Successful)
                    result = true;
                else
                    result = false;
                return result;
            }
        }

        public Result()
        {
            messages = new List<String>();
        }

        /// <summary>
        /// Adds a message to the result message queue.
        /// </summary>
        /// <param name="msg"></param>
        public void AddMessage(string msg)
        {
            messages.Add(msg);
        }

        public void ClearMessages()
        {
            messages.Clear();
        }

        public override string ToString() => SerializationService.Serialize(this);

        public static Result CombineResults(List<Result> resultList)
        {
            var result = new Result();
            var failures = new List<Result>();
            result.Type = ResultType.Successful;
            if (ListService.IsNonEmptyList(resultList))
            {
                foreach (var item in resultList)
                {
                    if (item != null && item.Type != ResultType.Successful)
                    {
                        result.Type = ResultType.Failure;
                        result.messages.AddRange(item.messages);
                        failures.Add(item);
                    }
                }
            }
            result.Obj = failures;
            return result;
        }
    }
}
