using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;


namespace EarnLogger
{
    public class LogManager
    {
        public Config GetSourceLogconfig(string sourceName)
        {
            var masterConfig = JsonFileReader.Read<Config>(@".\logConfig.json");
            var sourceLogConfig = JsonFileReader.Read<List<SourceLogConfig>>(@".\SourseLogConfig.json");

            // read master LogConfig and Source Log Config from EARNENGINE DB table app_parameter_store
            // Read Log config for Master EARN_LOGCONFIG_MASTER and source EARN_LOGCONFIG_BATCH_MANAGER from app_parameter_store

            var config = sourceLogConfig.Where(x => x.component == sourceName).Select(x => x.config).FirstOrDefault();
            return config== null ?  masterConfig: config;
        }
    }
  
    public class Api
    {
        public string level { get; set; }
        public bool publish { get; set; }
        public bool http { get; set; }
        public bool trace { get; set; }
        public bool data { get; set; }
    }

    public class Console
    {
        public string level { get; set; }
        public bool publish { get; set; }
        public bool http { get; set; }
        public bool trace { get; set; }
        public bool data { get; set; }
    }

    public class Db
    {
        public string level { get; set; }
        public bool publish { get; set; }
        public bool http { get; set; }
        public bool trace { get; set; }
        public bool data { get; set; }
    }

    public class File
    {
        public string level { get; set; }
        public bool publish { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public bool http { get; set; }
        public bool trace { get; set; }
        public bool data { get; set; }
    }

    public class Config
    {
        public Console console { get; set; }
        public File file { get; set; }
        public Api api { get; set; }
        public Db db { get; set; }
    }

    public class SourceLogConfig
    {
        public string component { get; set; }
        public Config config { get; set; }
    }

    public static class JsonFileReader
    {
        public static T Read<T>(string filePath)
        {
            string text = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text);
        }
    }
}
