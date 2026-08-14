namespace MaturaAi.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string TaskNumber { get; set; } = string.Empty;

        public string QuestionType { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public List<AnswerDto> Answers { get; set; }


    }
}
