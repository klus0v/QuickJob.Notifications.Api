using System.Net;
using MongoDB.Driver;
using QuickJob.Notifications.Core.Storages.Interfaces;
using QuickJob.Notifications.Core.Storages.Mongo;
using QuickJob.Notifications.DataModel.Entities.Mongo;
using QuickJob.Notifications.DataModel.Exceptions;
using Vostok.Logging.Abstractions;

namespace QuickJob.Notifications.Core.Storages;

public sealed class TemplatesStorage : ITemplatesStorage
{
    private readonly IMongoProvider mongoProvider;
    private readonly IMongoCollection<Template> templatesCollection;
    private readonly ILog log;

    public TemplatesStorage(IMongoProvider mongoProvider, IMongoCollection<Template> templatesCollection, ILog log)
    {
        this.mongoProvider = mongoProvider;
        this.templatesCollection = templatesCollection;
        this.log = log.ForContext(nameof(TemplatesStorage));
    }

    public async Task<Template> Get(string id)
    {
        var filter = new ExpressionFilterDefinition<Template>(t => t.Id == id);
        var templateEntity = await mongoProvider.Get(templatesCollection, filter);

        if (templateEntity is null)
        {
            log.Warn($"Template with id: '{id}' not found");
            throw new CustomHttpException(HttpStatusCode.NotFound, HttpErrors.TemplateNotFound(id));
        }
        
        return templateEntity;
    }
}