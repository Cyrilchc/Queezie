using System.Collections.Generic;

namespace Queezie.Models
{
    public class DisplayGameQuestionAnswerModel
    {
        public DisplayGameQuestionAnswerModel()
        {
        }

        public DisplayGameQuestionModel Question { get; set; }

        public List<DisplayAnswerModel> Answers { get; set; }
    }
}
