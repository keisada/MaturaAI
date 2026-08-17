namespace MaturaAi.DTOs
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ExamMonth { get; set; } = string.Empty;
        public int ExamYear { get; set; }
    }
}
