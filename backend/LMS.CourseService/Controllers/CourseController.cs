using LMS.CourseService.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
namespace LMS.CourseService.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly string _excelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Courses.xlsx");
        public CourseController(ILogger<CourseController> logger)
        {
            _logger = logger;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        [HttpGet]
        public IActionResult GetCourses()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var courseList = new List<CourseDto>();

                using var package = new ExcelPackage(new FileInfo(_excelPath));
                var sheet = package.Workbook.Worksheets.FirstOrDefault();

                if (sheet == null || sheet.Dimension == null)
                    return Ok(courseList); // return empty if no data

                for (int i = 2; i <= sheet.Dimension.Rows; i++) // skip header
                {
                    var course = new CourseDto
                    {
                        Id = int.TryParse(sheet.Cells[i, 1].Text, out var idVal) ? idVal : 0,
                        Title = sheet.Cells[i, 2].Text,
                        Description = sheet.Cells[i, 3].Text,
                        Instructor = sheet.Cells[i, 4].Text,
                        Category = sheet.Cells[i, 5].Text,
                        ContentHtml = sheet.Cells[i, 6].Text,
                        VideoUrl = sheet.Cells[i, 7].Text,
                        PdfFileName = sheet.Cells[i, 8].Text
                    };

                    courseList.Add(course);
                }

                return Ok(courseList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to load courses", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddCourse([FromBody] CourseDto dto)
        {
            using var package = new ExcelPackage(new FileInfo(_excelPath));
            var sheet = package.Workbook.Worksheets.FirstOrDefault() ?? package.Workbook.Worksheets.Add("Courses");

            int rowCount = sheet.Dimension?.Rows ?? 0;

            // Create header if not exists
            if (rowCount == 0)
            {
                sheet.Cells[1, 1].Value = "Id";
                sheet.Cells[1, 2].Value = "Title";
                sheet.Cells[1, 3].Value = "Description";
                sheet.Cells[1, 4].Value = "Instructor";
                sheet.Cells[1, 5].Value = "Category";
                sheet.Cells[1, 6].Value = "ContentHtml";
                sheet.Cells[1, 7].Value = "VideoUrl";
                sheet.Cells[1, 8].Value = "PdfFileName";
                rowCount = 1;
            }

            // Auto-increment course ID
            int newId = 1;
            for (int i = 2; i <= rowCount; i++)
            {
                int.TryParse(sheet.Cells[i, 1].Text, out int id);
                if (id >= newId) newId = id + 1;
            }

            int newRow = rowCount + 1;
            sheet.Cells[newRow, 1].Value = newId;
            sheet.Cells[newRow, 2].Value = dto.Title;
            sheet.Cells[newRow, 3].Value = dto.Description;
            sheet.Cells[newRow, 4].Value = dto.Instructor;
            sheet.Cells[newRow, 5].Value = dto.Category;
            sheet.Cells[newRow, 6].Value = dto.ContentHtml;
            sheet.Cells[newRow, 7].Value = dto.VideoUrl;
            sheet.Cells[newRow, 8].Value = dto.PdfFileName;

            package.Save();

            return Ok(new { message = "Course added successfully!", id = newId });
        }

        [HttpPut("{id}")]
        public IActionResult EditCourse(int id, [FromBody] CourseDto dto)
        {
            using var package = new ExcelPackage(new FileInfo(_excelPath));
            var sheet = package.Workbook.Worksheets.FirstOrDefault();

            if (sheet == null) return NotFound(new { message = "No data" });

            for (int i = 2; i <= sheet.Dimension.Rows; i++)
            {
                if (sheet.Cells[i, 1].Text == id.ToString())
                {
                    sheet.Cells[i, 2].Value = dto.Title;
                    sheet.Cells[i, 3].Value = dto.Description;
                    sheet.Cells[i, 4].Value = dto.Instructor;
                    sheet.Cells[i, 5].Value = dto.Category;
                    sheet.Cells[i, 6].Value = dto.ContentHtml;
                    sheet.Cells[i, 7].Value = dto.VideoUrl;
                    sheet.Cells[i, 8].Value = dto.PdfFileName;

                    package.Save();
                    return Ok(new { message = "Course updated successfully!" });
                }
            }

            return NotFound(new { message = "Course not found" });
        }
        // ❌ Delete Course
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            using var package = new ExcelPackage(new FileInfo(_excelPath));
            var sheet = package.Workbook.Worksheets.FirstOrDefault();

            if (sheet == null) return NotFound(new { message = "No data" });

            for (int i = 2; i <= sheet.Dimension.Rows; i++)
            {
                if (sheet.Cells[i, 1].Text == id.ToString())
                {
                    sheet.DeleteRow(i);
                    package.Save();
                    return Ok(new { message = "Course deleted" });
                }
            }

            return NotFound(new { message = "Course not found" });
        }
        [HttpPost("upload-pdf")]
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Pdfs");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName = file.FileName, message = "Uploaded" });
        }
    }
}
