using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.HAMQTT
{
    public static class Helpers
    {
        public static string ToJson(object obj)
        {
            if (obj is string str) return str;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
