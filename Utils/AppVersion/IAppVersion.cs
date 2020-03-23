namespace AppVersion
{
    public interface IAppVersion
    {
        string FileVersion { get; }

        string Version { get; }

        string InformationalVersion { get; }

        string OnlyMajorMinor { get; }
    }
}