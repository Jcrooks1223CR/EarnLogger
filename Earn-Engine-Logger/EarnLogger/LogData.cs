using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace EarnLogger
{
    public class LogData
    {
        public DateTime TimeStamp { get; set; }
        public string Host { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Correlationid { get; set; }
        public string Code { get; set; }
        public string Trace { get; set; }
        public string Data { get; set; }
        public string HTTP { get; set; }
    }

    public enum Level
    {
        error,
        warn,
        info,
        verbose,
        debug
    }
}
