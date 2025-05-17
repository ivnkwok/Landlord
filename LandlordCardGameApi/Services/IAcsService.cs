using LandlordCardGameApi.Models;

namespace LandlordCardGameApi.Services
{
    public interface IAcsService
    {
        Task<AcsUser> CreateUser(string userName, UserRoles role);

        Task<string> CreateChat(AcsUser acsUser);

        Task<bool> AddUserToChat(AcsUser acsUser, string threadId, string threadOwnerToken);

        Task RevokeAcsToken(string userId);

        Task DeleteChatThread(string threadId, string threadOwnerToken);
    }
}
