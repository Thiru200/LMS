using Microsoft.AspNetCore.Mvc;

namespace LMS.ProgressService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly ILogger<ProgressController> _logger;
        public ProgressController(ILogger<ProgressController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetUser")]
        public string Get()
        {
            return "Hello World";
        }
    }
}