using Microsoft.AspNetCore.Mvc;
namespace LMS.UserService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet(Name = "GetUser")]
        public string Get()
        {
            return "Hello World";
        }
    }
}