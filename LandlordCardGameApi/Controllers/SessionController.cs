using Azure.Core;
using LandlordCardGameApi.Models;
using LandlordCardGameApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LandlordCardGameApi.Controllers
{
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly IAcsService acsService;
        private readonly ISessionStorageService sessionStorageService;

        public SessionController(ILogger<SessionController> logger, IAcsService acsService, ISessionStorageService sessionStorageService)
        {
            _logger = logger;
            this.acsService = acsService;
            this.sessionStorageService = sessionStorageService;
        }

        [Route("session/createsession")]
        [HttpPost]
        public async Task<ActionResult> CreateSession(string userName)
        {
            _logger.LogInformation("Inside Create Session");

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogInformation("User name is null or empty.");
                return BadRequest();
            }

            var acsUser = await this.acsService.CreateUser(userName, UserRoles.Owner);
            //var acsUser = new AcsUser { UserName = userName, Role = UserRoles.Owner, Token="MockToken", UserId = "MockUserId" };
            _logger.LogInformation("Created AcsUserId:" + acsUser.UserId);

            var acsConnectionId = await this.acsService.CreateChat(acsUser);
            //var acsConnectionId = Guid.NewGuid().ToString();
            _logger.LogInformation("Created AcsConnectionId:" + acsConnectionId);

            var sessionInfo = new SessionInfo { AcsConnectionId = acsConnectionId, AcsUsers = new List<AcsUser> { acsUser }, RoomStatus = RoomStatus.WaitPlayers };
            sessionInfo = await this.sessionStorageService.Create(sessionInfo);
            var result = new
            {
                PartitionKey = sessionInfo.PartitionKey,
                RoomId = sessionInfo.RoomId,
                AcsUser = acsUser,
                AcsConnectionId = sessionInfo.AcsConnectionId,
                AcsEndpoint= this.acsService.Endpoint.Trim('/'),
                RoomStatus = sessionInfo.RoomStatus.ToString(),
            };

            _logger.LogInformation("Create Session Successfully");
            return Ok(result);
        }

        [Route("session/createanddeletesession")]
        [HttpPost]
        public async Task<ActionResult> CreateAndDeleteSession(string userName)
        {
            var result = await this.CreateSession(userName);
            var okResult = result as OkObjectResult;
            if (okResult != null)
            {
                var value = okResult.Value;
                string pkey = (string)value.GetType().GetProperty("PartitionKey").GetValue(value);
                string roomId = (string)value.GetType().GetProperty("RoomId").GetValue(value);
                Thread.Sleep(1000);
                this.EndSession(pkey, roomId);
            }

            return Ok(result);
        }

        [Route("session/endsession")]
        [HttpPost]
        public async Task<ActionResult> EndSession(string pkey, string roomId)
        {
            if (string.IsNullOrWhiteSpace(pkey))
            {
                _logger.LogInformation("PartitionKey is null or empty.");
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(roomId))
            {
                _logger.LogInformation("RoomId is null or empty.");
                return BadRequest();
            }

            var sessionInfo = await this.sessionStorageService.Retrieve(pkey, roomId);

            var tasks = new List<Task>();
            foreach (var acsUser in sessionInfo.AcsUsers)
            {
                if (acsUser.Role == UserRoles.Owner)
                {
                    await acsService.DeleteChatThread(sessionInfo.AcsConnectionId, acsUser.Token);
                }

                if (!string.IsNullOrWhiteSpace(acsUser.UserId))
                {
                    tasks.Add(acsService.RevokeAcsToken(acsUser.UserId));
                }
            }

            await Task.WhenAll(tasks);

            sessionInfo.RoomStatus = RoomStatus.Closed;
            await this.sessionStorageService.Update(sessionInfo);

            return this.Ok();
        }

        [Route("session/joinsession")]
        [HttpPost]
        public async Task<ActionResult> JoinSession(string roomId, string userName)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                _logger.LogInformation("RoomId is null or empty.");
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogInformation("User name is null or empty.");
                return BadRequest();
            }

            try
            {
                var sessionInfo = await this.sessionStorageService.Retrieve(roomId);
                string threadOwnerToken = "";

                //Get owner token
                foreach (AcsUser user in sessionInfo.AcsUsers)
                {
                    if (user.Role == UserRoles.Owner)
                    {
                        threadOwnerToken = user.Token;
                    }
                }

                //Ensure owner token exists
                if (string.IsNullOrWhiteSpace(threadOwnerToken))
                {
                    _logger.LogInformation("Could not find owner token.");
                    return BadRequest();
                }

                var acsUser = await this.acsService.CreateUser(userName, UserRoles.Guest);
                _logger.LogInformation("Created guest user");
                sessionInfo.AcsUsers.Add(acsUser);
                _logger.LogInformation("Added guest user to AcsUsers");
                await this.acsService.AddUserToChat(acsUser, sessionInfo.AcsConnectionId, threadOwnerToken);
                _logger.LogInformation("Added guest user to chat");
                await this.sessionStorageService.Update(sessionInfo);

                return this.Ok();
            }
            catch (Exception)
            {
                return this.NotFound();
            }
            
        }
    }
}
