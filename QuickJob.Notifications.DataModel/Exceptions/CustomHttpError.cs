namespace QuickJob.Notifications.DataModel.Exceptions;

public sealed record CustomHttpError(string? Code, string? Message = null)
{
    
}
