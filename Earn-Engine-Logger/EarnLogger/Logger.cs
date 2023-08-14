using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using File = System.IO.File;
using System.Data.SqlClient;
using System.Net;

namespace EarnLogger
{
    public class Logger
    {
        private static void Log(Level level, LogData logData, string code)
        {
            try
            {
                if (logData.Source == null)
                    Environment.Exit(0);
                // Add UTC time Zone
                logData.TimeStamp = DateTime.UtcNow;
                LogManager logManager = new LogManager();
                Config sourceConfigLog = logManager.GetSourceLogconfig(logData.Source);

                int requestLevel = (int)level;

                if (sourceConfigLog.console.publish == true && !string.IsNullOrEmpty(sourceConfigLog.console.level))
                {
                    int configLevel = (int)(Enum.Parse<Level>(sourceConfigLog.console.level));
                    if (requestLevel > configLevel)
                    {
                        sourceConfigLog.console.publish = false;
                    }
                    ToConsole(logData, sourceConfigLog);
                }

                if (sourceConfigLog.file.publish == true && !string.IsNullOrEmpty(sourceConfigLog.file.level))
                {
                    int configLevel = (int)(Enum.Parse<Level>(sourceConfigLog.file.level));
                    if (requestLevel > configLevel)
                    {
                        sourceConfigLog.file.publish = false;
                    }
                    ToFile(logData, sourceConfigLog);
                }

                if (sourceConfigLog.api.publish == true && !string.IsNullOrEmpty(sourceConfigLog.api.level))
                {
                    int configLevel = (int)(Enum.Parse<Level>(sourceConfigLog.api.level));
                    if (requestLevel > configLevel)
                    {
                        sourceConfigLog.api.publish = false;
                    }
                    ToAPI(logData, sourceConfigLog);
                }

                if (sourceConfigLog.db.publish == true && !string.IsNullOrEmpty(sourceConfigLog.db.level))
                {
                    int configLevel = (int)(Enum.Parse<Level>(sourceConfigLog.db.level));
                    if (requestLevel > configLevel)
                    {
                        sourceConfigLog.db.publish = false;
                    }
                    ToDB(logData, sourceConfigLog);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void Error(LogData logData, string code)
        {
            if (logData == null)
            {
                Environment.Exit(0);
            }
            Log(Level.error, logData, code);
        }

        public static void Warn(LogData logData, string code)
        {
            if (logData == null)
            {
                Environment.Exit(0);
            }
            Log(Level.warn, logData, code);
        }

        public static void Info(LogData logData, string code)
        {
            if (logData == null)
            {
                Environment.Exit(0);
            }
            Log(Level.info, logData, code);
        }

        public static void Verbose(LogData logData, string code)
        {
            if (logData == null)
            {
                Environment.Exit(0);
            }
            Log(Level.verbose, logData, code);
        }

        public static void Debug(dynamic logData, string code)
        {
            if (logData == null)
            {
                Environment.Exit(0);
            }
            Log(Level.debug, logData, code);
        }

        private static void ToConsole(LogData logData, Config logConfig)
        {
            string logMessage = string.Empty;
            if (logConfig.console.publish == true)
            {
                if (logConfig.console.http == false)
                {
                    logData.HTTP = string.Empty;
                }
                if (logConfig.console.trace == false)
                {
                    logData.Trace = string.Empty;
                }
                if (logConfig.console.data == false)
                {
                    logData.Data = string.Empty;
                }

                logMessage = JsonConvert.SerializeObject(logData);
                System.Console.WriteLine("Log to Console {0}:", logMessage);
            }

            //Log to Console
        }

        private static void ToAPI(LogData logData, Config logConfig)
        {
            string logMessage = string.Empty;

            if (logConfig.api.publish == true)
            {
                if (logConfig.console.http == false)
                {
                    logData.HTTP = string.Empty;
                }
                if (logConfig.console.trace == false)
                {
                    logData.Trace = string.Empty;
                }
                if (logConfig.console.data == false)
                {
                    logData.Data = string.Empty;
                }

                logMessage = JsonConvert.SerializeObject(logData);
                System.Console.WriteLine("Log to Api {0}:", logMessage);

            }
              
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.BaseAddress = "https://devakafkacon01/errorlogging/logger.asmx?op=LogError";
                    var url = "/posts";
                    string data = JsonConvert.SerializeObject(logData);
                    var response = webClient.UploadString(url, data);
                    var result = JsonConvert.DeserializeObject<object>(response);
                    System.Console.WriteLine("Log to Api {0}:", result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Log to API
            //https://devakafkacon01/errorlogging/logger.asmx?op=LogError
        }

        private static void ToFile(LogData logData, Config logConfig)
        {
            string logMessage = string.Empty;

            if (logConfig.file.publish == true)
            {
                if (logConfig.file.http == false)
                {
                    logData.HTTP = string.Empty;
                }
                if (logConfig.file.trace == false)
                {
                    logData.Trace = string.Empty;
                }
                if (logConfig.file.data == false)
                {
                    logData.Data = string.Empty;
                }

                logMessage = JsonConvert.SerializeObject(logData);
                //System.Console.WriteLine("Log to File {0}:", logMessage);
              
                string logFile = logData + "_Logs.txt";
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(logFile))
                    {
                        streamWriter.WriteLine(" Log to File :{0}", logMessage);
                        streamWriter.Close();
                    }
                }
                catch (Exception)
                { 
                }
                
            }
        }
            // Log to File
        

        private static void ToDB(LogData logData, Config logConfig)
        {
            string logMessage = string.Empty;

            if (logConfig.db.publish == true)
            {
                if (logConfig.db.http == false)
                {
                    logData.HTTP = string.Empty;
                }
                if (logConfig.db.trace == false)
                {
                    logData.Trace = string.Empty;
                }
                if (logConfig.db.data == false)
                {
                    logData.Data = string.Empty;
                }
                
                logMessage = JsonConvert.SerializeObject(logData);
                System.Console.WriteLine("Log to DB {0}:", logMessage);

                using (SqlConnection connection = new SqlConnection("Data Source=(logData);Initial Catalog=Northwind;Integrated Security=SSPI;"))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Logs (HTTP, trace, Data) VALUES (@HTTP, @Trace, @Data)";
                    command.Parameters.AddWithValue("@Http", logData.HTTP);
                    command.Parameters.AddWithValue("@Trace", logData.Trace);
                    command.Parameters.AddWithValue("@Data", logData.Data);
                    command.ExecuteNonQuery();
                }
                

              
               
            }
            //Log to DB
        }
    }
}