using System;
using System.IO;
using AppRunner;
using EfCore.Common;
#pragma warning disable 1591

namespace EasyRates.ReaderApp.AspNet.Di
{
    public class AppConfig:IAppConfig
    {
        
        public string ApplicationName { get; set; }
        
        /// <summary>
        /// Folder with executable files of application.
        /// Auto define.
        /// </summary>
        public string BinFolder { get; set; }
        
        /// <summary>
        /// Folder contains the configuration files.
        /// From command line.
        /// </summary>
        public string ConfigFolder { get; set; }
        
        public string LogsFolder { get; set; }
        
        public string DataFolder { get; set; }
        
        public string[] Urls { get; set; }


        [RequiredSetting]
        public string ConnectionString { get; set; }
        
        [RequiredSetting]
        public DbType? DbType { get; set; }
        
        
        

        public virtual void SetDefaultIfNeed()
        {
            ApplicationName = ApplicationName ?? "EasyRates";
            LogsFolder = LogsFolder ?? Path.Combine(Directory.GetCurrentDirectory(), "logs");
            DataFolder = DataFolder ?? Path.Combine(Directory.GetCurrentDirectory(), "data");
            Urls = Urls ?? new [] {"http://localhost:5000"};
            
        }
    }
}