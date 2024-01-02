using MongoDB.Driver;

namespace QuickJob.Notifications.Core.Storages.Mongo;

public interface IMongoProvider
{
    Task<TEntity> Get<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter);
    Task Write<TEntity>(IMongoCollection<TEntity> mongoCollection, TEntity writeRequest);
    Task Patch<TEntity>(IMongoCollection<TEntity> mongoCollection, TEntity entity, ExpressionFilterDefinition<TEntity> filter);
    Task Update<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateRequest);
    Task BatchUpdate<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateRequest);
    Task Remove<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter);
    Task<long> GetCount<TEntity>(IMongoCollection<TEntity> mongoCollection, ExpressionFilterDefinition<TEntity> filter);
}