using Newtonsoft.Json.Linq;

namespace EarnLogger
{
    public class EarnLogger
    {
        public static void Main(string[] args)
        {
            Logger.Error(new LogData() { Host="Server Name",Correlationid= Guid.NewGuid().ToString(),Data="Data",HTTP="http",Message="Error Message",Source= "TEST_V2_BATCH_MANAGER",Trace="Message logged successfuly" },"code1");

            // Logger.Info(new LogData() { Host="Server Name",Correlationid= Guid.NewGuid().ToString(),Data="Data",HTTP="http",Message="Info Message",Source= "TEST_V2_BATCH_MANAGER",Trace="Message logged successfuly" },"code1");
            //Logger.Debug(new LogData() { Host = "Server Name", Correlationid = Guid.NewGuid().ToString(), Data = "Data", HTTP = "http", Message = "Debug Message", Source = "TEST_V2_BATCH_MANAGER", Trace = "Message logged successfuly" }, "code1");

            //Logger.Error(new LogData() { Host="Server Name",Correlationid= Guid.NewGuid().ToString(),Data="Data",HTTP="http",Message="Error Message",Source= "TEST_V2_INVALID_SOURCE",Trace="Message logged successfuly" },"code1");
            System.Console.ReadLine();
        }
    }
}
