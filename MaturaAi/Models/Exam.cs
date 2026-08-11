using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturaAi.Models;

[Table("Exam", Schema = "dbo")]
public class Exam
{
    [Key]
    public int Id { get; set; }

    public string Subject { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ExamMonth { get; set; } = string.Empty;
    public int ExamYear { get; set; }
}