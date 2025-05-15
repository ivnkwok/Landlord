using Microsoft.AspNetCore.Mvc;

namespace LandlordCardGameApi.Controllers
{
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [Route("session/createsession")]
        [HttpGet]
        public string CreateSession()
        {
            _logger.LogInformation("Inside Create Session");
            return Guid.NewGuid().ToString();
        }
    }
}
