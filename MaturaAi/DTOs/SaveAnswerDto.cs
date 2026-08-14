namespace MaturaAi.DTOs
{
    public class SaveAnswerDto
    {
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string? AnswerText { get; set; }
    }
}
