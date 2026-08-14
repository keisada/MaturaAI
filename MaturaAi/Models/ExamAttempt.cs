namespace MaturaAi.Models
{
    public class ExamAttempt
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime StartedAt {  get; set; }
        public DateTime? EndAt { get; set; }
        public int Score { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }

    }
}
