using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturaAi.Models;

[Table("Questions", Schema = "dbo")]
public class Question
{
    [Key]
    public int Id { get; set; }

    public int ExamId { get; set; }

    public string TaskNumber { get; set; } = string.Empty;

    public string QuestionType { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}