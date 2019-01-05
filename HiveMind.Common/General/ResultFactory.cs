using HiveMind.Common.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiveMind.Common.General
{
    public class ResultFactory
    {
        public static IResult CreateInstance()
        {
            return new Result();
        }
    }
}
