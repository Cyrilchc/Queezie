namespace Queezie.Models
{
    public class DisplayQuizModel
    {
        public DisplayQuizModel()
        {
        }

        public DisplayQuizModel(string id, string quiz, int duration)
        {
            Id = id;
            Quiz = quiz;
            Duration = duration;
        }

        /// <summary>
        /// Gets or sets the quiz id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the quiz name.
        /// </summary>
        public string Quiz { get; set; }

        /// <summary>
        /// Gets or sets the quiz duration.
        /// </summary>
        public int Duration { get; set; }
    }
}
