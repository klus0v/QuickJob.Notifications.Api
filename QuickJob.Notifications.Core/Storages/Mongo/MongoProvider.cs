using System.Net;
using MongoDB.Driver;
using QuickJob.Notifications.DataModel.Exceptions;
using Vostok.Logging.Abstractions;

namespace QuickJob.Notifications.Core.Storages.Mongo;

public sealed class MongoProvider : IMongoProvider
{
    private readonly ILog log;

    public MongoProvider(ILog log)
    {
        this.log = log.ForContext(nameof(MongoProvider));
    }

    public async Task<TEntity> Get<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter)
    {
        try 
        {
            var result = await mongoCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e) 
        {
            log.Error($"Error find entity: {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }

    public async Task Write<TEntity>(IMongoCollection<TEntity> mongoCollection, TEntity entity)
    { 
        try 
        {
            await mongoCollection.InsertOneAsync(entity);
        }
        catch (Exception e) 
        {
            log.Error($"Error write entity: {entity}, {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
        
    }

    public async Task Patch<TEntity>(IMongoCollection<TEntity> mongoCollection, TEntity entity, ExpressionFilterDefinition<TEntity> filter)
    {
        try
        {
            await mongoCollection.ReplaceOneAsync(filter, entity);
        }
        catch (Exception e) 
        {
            log.Error($"Error patch entity: {entity}, {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }

    public async Task Update<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateRequest)
    {
        try
        {
            await mongoCollection.UpdateOneAsync(filter, updateRequest);
        }
        catch (Exception e) 
        {
            log.Error($"Error update entity: {updateRequest}, {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }

    public async Task BatchUpdate<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateRequest)
    {
        try
        {
            await mongoCollection.UpdateManyAsync(filter, updateRequest);
        }
        catch (Exception e) 
        {
            log.Error($"Error update entities: {updateRequest}, {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }

    public async Task Remove<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter)
    {
        try 
        {
            await mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception e) 
        {
            log.Error($"Error deleting entity: {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }

    public async Task<long> GetCount<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter)
    {
        try
        {
            var count = await mongoCollection.CountDocumentsAsync(filter);
            return count;
        }
        catch (Exception e) 
        {
            log.Error($"Get count error: {e.Message}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Mongo(e.Message));
        }
    }
}