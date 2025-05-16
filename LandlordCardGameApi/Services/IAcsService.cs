using LandlordCardGameApi.Models;

namespace LandlordCardGameApi.Services
{
    public interface IAcsService
    {
        Task<AcsUser> CreateUser();

        Task<string> CreateChat(AcsUser acsUser, string displayName);

        Task<bool> AddUserToChat(AcsUser acsUser, string displayName, string threadId, string threadOwnerToken);

        Task RevokeAcsToken(string id);

        Task DeleteChatThread(string threadId, string threadOwnerToken);
    }
}
