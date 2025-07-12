using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
namespace LMS.UserService.Controllers
{

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public UserController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        [HttpGet(Name = "GetUser")]
        public string Get()
        {
            return "Hello World";
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Users.xlsx");
            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(500, new { message = "User data not found" });
            }
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; // assuming first sheet
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // skip header row
            {
                var email = worksheet.Cells[row, 1].Text.Trim();     // Column A
                var password = worksheet.Cells[row, 2].Text.Trim();  // Column B
                var role = worksheet.Cells[row, 3].Text.Trim();       // Column C
                var id = worksheet.Cells[row, 4].Text.Trim();         // Column D

                if (email == request.Email && password == request.Password)
                {
                    var user = new User
                    {
                        Id = int.Parse(id),
                        Email = email,
                        Password = password,
                        Role = role
                    };

                    var token = _jwtService.GenerateToken(user);
                    return Ok(new
                    {
                        token,
                        user = new { user.Id, user.Email, user.Role }
                    });
                }
            }
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }
}