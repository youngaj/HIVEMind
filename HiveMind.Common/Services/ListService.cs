using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiveMind.Common.Services
{
    public class ListService
    {
        public static bool IsEmptyList(IEnumerable<object> list)
        {
            var result = false;
            if (list == null || list.Any() == false)
                result = true;
            return result;
        }

        public static bool IsNonEmptyList(IEnumerable<object> list)
        {
            return list != null && list.Any();
        }

        public static bool IsEmptyList(IQueryable<object> list)
        {
            var result = false;
            if (list == null || list.Any() == false)
                result = true;
            return result;
        }

        public static bool IsNonEmptyList(IQueryable<object> list)
        {
            return list != null && list.Any();
        }
    }
}
