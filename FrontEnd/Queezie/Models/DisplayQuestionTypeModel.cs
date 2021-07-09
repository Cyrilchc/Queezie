namespace Queezie.Models
{
    public class DisplayQuestionTypeModel
    {
        public DisplayQuestionTypeModel()
        {
        }

        public DisplayQuestionTypeModel(string id, string questionType)
        {
            Id = id;
            QuestionType = questionType;
        }

        public string Id { get; set; }

        public string QuestionType { get; set; }
    }
}
