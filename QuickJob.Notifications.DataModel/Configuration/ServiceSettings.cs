namespace QuickJob.Notifications.DataModel.Configuration;

public sealed record ServiceSettings
{
    public List<string> Origins { get; set; }
    public string UsersApiKey { get; set; }
    public string UsersApiBaseUrl { get; set; }

    public HashSet<string> AllowedApiKeys { get; set; }
    public string FrontRegisterUrl { get; set; }
}