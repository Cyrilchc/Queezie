namespace Queezie.Models
{
    public class DisplayQuestionModel
    {
        public DisplayQuestionModel()
        {
        }

        public DisplayQuestionModel(string id, string question, string questionTypeId, string domainId)
        {
            Question = question;
            QuestionTypeId = questionTypeId;
            DomainId = domainId;
            Id = id;
        }

        /// <summary>
        /// Gets or Sets the question id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets the question phrase.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or Sets the question type.
        /// </summary>
        public string QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the question domain.
        /// </summary>
        public string DomainId { get; set; }
    }
}
