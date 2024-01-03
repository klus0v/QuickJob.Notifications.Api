using System.Threading.Tasks;
using QuickJob.Notifications.Client.Models;

namespace QuickJob.Notifications.Client;

public interface IRequestSender
{
    Task<ApiResult<TResponse>> SendRequestAsync<TResponse>(
        string httpMethod,
        string uri,
        object requestEntity = null)
        where TResponse : class;

    Task<ApiResult> SendRequestAsync(
        string httpMethod,
        string uri,
        object requestEntity = null);
}
