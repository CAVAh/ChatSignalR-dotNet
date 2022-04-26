using Microsoft.AspNetCore.Mvc;

namespace ChatSignalR.Controllers
{

        [ApiController]
        [Route("[controller]")]
        public class ChatSignalRController : ControllerBase
        {
            [HttpGet]
            public String Get()
            {
                return "{Bem-vindo ao Xico}";
            }
        }
}
