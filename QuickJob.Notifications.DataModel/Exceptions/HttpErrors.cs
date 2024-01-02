namespace QuickJob.Notifications.DataModel.Exceptions;

public static class HttpErrors
{
    private const string UsersApiError = "UsersApiError";
    private const string MongoError = "MongoError";
    private const string SmtpError = "SmtpError";
    private const string NotFoundError = "NotFound";
    private const string NoAccessError = "NoAccess";
    private const string UnauthorizedApiError = "UnauthorizedApi";
    private const string NoAccessToApiError = "NoAccessToApi";
    private const string TemplateNotFoundError = "TemplateNotFound";

    public static CustomHttpError Users(object error) => new(UsersApiError, $"Users api error: {error}");
    public static CustomHttpError NotFound(object itemKey) => new(NotFoundError, $"Not found item with key: '{itemKey}'");
    public static CustomHttpError UnauthorizedApi => new(UnauthorizedApiError, "Api is unauthorized");
    public static CustomHttpError NoAccessToApi => new(NoAccessToApiError, "No access to api'");
    public static CustomHttpError NoAccess(object itemKey) => new(NoAccessError, $"No access to item with key: '{itemKey}'");
    public static CustomHttpError Mongo(object error) => new(MongoError, $"MongoDB error: {error}");
    public static CustomHttpError TemplateNotFound(string id) => new(TemplateNotFoundError, $"Template with id: '{id}' not found");
    public static CustomHttpError Smtp(Guid requestId) => new(SmtpError, $"Error send message by email, id: '{requestId}'");
}
