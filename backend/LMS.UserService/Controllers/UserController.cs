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
                var id = worksheet.Cells[row, 1].Text.Trim();           // Column A
                var name = worksheet.Cells[row, 2].Text.Trim();         // Column B
                var email = worksheet.Cells[row, 3].Text.Trim();        // Column C
                var password = worksheet.Cells[row, 4].Text.Trim();     // Column D
                var role = worksheet.Cells[row, 5].Text.Trim();         // Column E
                if (email == request.Email && password == request.Password)
                {
                    var user = new User
                    {
                        Id = int.Parse(id),
                        Email = email,
                        Name = name,
                        Password = password,
                        Role = role
                    };
                    var token = _jwtService.GenerateToken(user);
                    return Ok(new
                    {
                        token,
                        user = new { user.Id, user.Name, user.Email, user.Role }
                    });
                }
            }
            return Unauthorized(new { message = "Invalid email or password" });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string excelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Users.xlsx");

                using (var package = new ExcelPackage(new FileInfo(excelPath)))
                {
                    var sheet = package.Workbook.Worksheets.FirstOrDefault()
                                ?? package.Workbook.Worksheets.Add("Users");

                    int rowCount = sheet.Dimension?.Rows ?? 0;

                    // Create header if empty
                    if (rowCount == 0)
                    {
                        sheet.Cells[1, 1].Value = "Id";
                        sheet.Cells[1, 2].Value = "Username";
                        sheet.Cells[1, 3].Value = "Email";
                        sheet.Cells[1, 4].Value = "Password";
                        sheet.Cells[1, 5].Value = "Role";
                        rowCount = 1;
                    }

                    // Auto-generate ID by checking the last row
                    int newId = 1;
                    if (rowCount > 1)
                    {
                        for (int i = 2; i <= rowCount; i++)
                        {
                            int.TryParse(sheet.Cells[i, 1].Text, out int lastId);
                            if (lastId >= newId)
                                newId = lastId + 1;
                        }
                    }

                    // Write new row
                    int newRow = rowCount + 1;
                    sheet.Cells[newRow, 1].Value = newId;
                    sheet.Cells[newRow, 2].Value = dto.Name;
                    sheet.Cells[newRow, 3].Value = dto.Email;
                    sheet.Cells[newRow, 4].Value = dto.Password; // ❗ Hash this in real apps
                    sheet.Cells[newRow, 5].Value = dto.Role;

                    package.Save();
                }

                return Ok(new { message = "User registered successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to register user", error = ex.Message });
            }
        }

    }
}