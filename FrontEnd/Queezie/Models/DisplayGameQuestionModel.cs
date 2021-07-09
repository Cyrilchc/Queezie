namespace Queezie.Models
{
    public class DisplayGameQuestionModel
    {
        public DisplayGameQuestionModel()
        {
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
        public string QuestionType { get; set; }
    }
}
