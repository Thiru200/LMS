namespace LMS.CourseService.Models;

public class CourseDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Instructor { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? ContentHtml { get; set; } // For blog-style content (rich text)
    public string? VideoUrl { get; set; } // Optional
    public string? PdfFileName { get; set; } // e.g., "intro.pdf"
}