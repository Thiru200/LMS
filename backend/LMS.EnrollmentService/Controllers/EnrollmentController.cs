using Microsoft.AspNetCore.Mvc;
namespace LMS.EnrollmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly ILogger<EnrollmentController> _logger;
    public EnrollmentController(ILogger<EnrollmentController> logger)
    {
        _logger = logger;
    }
    [HttpGet(Name = "GetUser")]
    public string Get()
    {
        return "Hello World";
    }
}
