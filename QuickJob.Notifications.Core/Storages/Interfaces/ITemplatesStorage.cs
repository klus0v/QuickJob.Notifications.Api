using QuickJob.Notifications.DataModel.Entities.Mongo;

namespace QuickJob.Notifications.Core.Storages.Interfaces;

public interface ITemplatesStorage 
{
    Task<Template> Get(string id);
}