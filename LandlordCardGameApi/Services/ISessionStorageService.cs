using LandlordCardGameApi.Models;

namespace LandlordCardGameApi.Services
{
    public interface ISessionStorageService
    {
        Task<SessionInfo> Create(SessionInfo sessionInfo);

        Task<SessionInfo> Retrieve(string rowKey);

        Task<SessionInfo> Retrieve(string partitionKey, string rowKey);

        Task Update(SessionInfo sessionInfo);
    }
}
