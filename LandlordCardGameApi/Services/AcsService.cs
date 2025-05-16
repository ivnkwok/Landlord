using LandlordCardGameApi.Models;
using LandlordCardGameApi.Settings;
using Microsoft.Extensions.Options;
using System.Net;

namespace LandlordCardGameApi.Services
{
    public class AcsService: IAcsService
    {
        private readonly AcsSettings _settings;

        public AcsService(IOptions<AcsSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<AcsUser> CreateUser()
        {
            //var client = new CommunicationIdentityClient(_settings.ConnectionString);
            //var response = await client.CreateUserAndTokenAsync(scopes: new[] { CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP });

            //if (response.GetRawResponse().Status != (int)HttpStatusCode.Created)
            //{
            //    throw new Exception(response.GetRawResponse().ReasonPhrase);
            //}

            //var acsUser = new AcsUser() { UserId = response.Value.User.Id, Token = response.Value.AccessToken.Token };
            //return acsUser;
            throw new NotImplementedException();
        }

        public async Task<string> CreateChat(AcsUser acsUser, string displayName)
        {
            //CommunicationTokenCredential tokenCredential = new CommunicationTokenCredential(acsUser.Token);
            //Uri endpoint = new Uri(_settings.ServiceEndpoint);
            //ChatClient chatClient = new ChatClient(endpoint, tokenCredential);

            //var participant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: acsUser.UserId))
            //{
            //    DisplayName = displayName
            //};

            //var response = await chatClient.CreateChatThreadAsync(topic: "Quick Assist Chat", participants: new[] { participant });

            //if (response.GetRawResponse().Status != (int)HttpStatusCode.Created)
            //{
            //    throw new Exception(response.GetRawResponse().ReasonPhrase);
            //}

            //return response.Value.ChatThread.Id;
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserToChat(AcsUser acsUser, string displayName, string threadId, string threadOwnerToken)
        {
            //CommunicationTokenCredential tokenCredential = new CommunicationTokenCredential(threadOwnerToken);
            //Uri endpoint = new Uri(_settings.ServiceEndpoint);
            //ChatClient chatClient = new ChatClient(endpoint, tokenCredential);

            //var participant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: acsUser.UserId))
            //{
            //    DisplayName = displayName
            //};

            //var chatThreadClient = chatClient.GetChatThreadClient(threadId);
            //var response = await chatThreadClient.AddParticipantAsync(participant);

            //if (response.Status != (int)HttpStatusCode.Created)
            //{
            //    throw new Exception(response.ReasonPhrase);
            //}

            //return true;
            throw new NotImplementedException();
        }

        public async Task RevokeAcsToken(string id)
        {
            //var client = new CommunicationIdentityClient(_settings.ConnectionString);
            //var identity = new CommunicationUserIdentifier(id);
            //var response = await client.RevokeTokensAsync(identity);

            //if (response.Status != (int)HttpStatusCode.NoContent)
            //{
            //    throw new Exception(response.ReasonPhrase);
            //}
            throw new NotImplementedException();
        }

        public async Task DeleteChatThread(string threadId, string threadOwnerToken)
        {
            //CommunicationTokenCredential tokenCredential = new CommunicationTokenCredential(threadOwnerToken);
            //Uri endpoint = new Uri(this.ServiceEndpoint);
            //ChatClient chatClient = new ChatClient(endpoint, tokenCredential);
            //var response = await chatClient.DeleteChatThreadAsync(threadId);
            //if (response.Status != (int)HttpStatusCode.NoContent)
            //{
            //    throw new Exception(response.ReasonPhrase);
            //}
            throw new NotImplementedException();
        }
    }
}
