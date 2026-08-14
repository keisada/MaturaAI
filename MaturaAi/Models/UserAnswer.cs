namespace MaturaAi.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int ExamAttemptId { get; set; }
        public int QuestionId { get; set; }

        public int? AnswerId { get; set; }  

        public string? AnswerText { get; set; }
    }
}
