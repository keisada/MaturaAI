using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaturaAi.Models;

[Table("Answers", Schema = "dbo")]
public class Answer
{
    [Key]
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}