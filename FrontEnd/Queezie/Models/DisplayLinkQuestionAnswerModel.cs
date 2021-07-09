namespace Queezie.Models
{
    public class DisplayLinkQuestionAnswerModel
    {
        public DisplayLinkQuestionAnswerModel()
        {
        }

        public DisplayLinkQuestionAnswerModel(string id, string questionId, string answerId)
        {
            Id = id;
            QuestionId = questionId;
            AnswerId = answerId;
        }

        public string Id { get; set; }

        public string QuestionId { get; set; }

        public string AnswerId { get; set; }
    }
}
