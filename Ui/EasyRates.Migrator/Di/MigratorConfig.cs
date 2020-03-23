using System;
using System.IO;
using AppRunner;
using EfCore.Common;

namespace EasyRates.Migrator.Di
{
    public class MigratorConfig:IAppConfig
    {
        
        public string ApplicationName { get; set; }
        
        /// <summary>
        /// Folder with executable files of application.
        /// Auto define.
        /// </summary>
        public string BinFolder { get; set; }
        
        public string LogsFolder { get; set; }
        
        public string DataFolder { get; set; }
        
        
        [RequiredSetting]
        public string ConnectionString { get; set; }
        
        [RequiredSetting]
        public DbType? DbType { get; set; }
        
        public virtual void SetDefaultIfNeed()
        {
            ApplicationName = ApplicationName ?? "EasyRates";
            LogsFolder = LogsFolder ?? Path.Combine(Directory.GetCurrentDirectory(), "logs");
            DataFolder = DataFolder ?? Path.Combine(Directory.GetCurrentDirectory(), "data");
        }
    }
}