namespace Queezie.Models
{
    public class DisplayLinkQuizQuestionModel
    {
        public DisplayLinkQuizQuestionModel()
        {
        }

        public DisplayLinkQuizQuestionModel(string id, string quizId, string questionId)
        {
            Id = id;
            QuizId = quizId;
            QuestionId = questionId;
        }

        public string Id { get; set; }

        public string QuizId { get; set; }

        public string QuestionId { get; set; }
    }
}
