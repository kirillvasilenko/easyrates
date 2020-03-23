namespace AppRunner
{
    public interface IAppConfig
    {
        string ApplicationName { get; }
        
        string LogsFolder { get; }
        
        string DataFolder { get; }
        
        /// <summary>
        /// Folder with executable files of application.
        /// Auto define.
        /// </summary>
        string BinFolder { get; }
    }
}