namespace JupyterSharp.Dto;

public sealed class VersionInfo
{
    public VersionInfo(string version)
    {
        Version = version;
    }

    public string Version { get; }
}
