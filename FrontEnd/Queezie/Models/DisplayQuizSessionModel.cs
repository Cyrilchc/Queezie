using System;

namespace Queezie.Models
{
    public class DisplayQuizSessionModel
    {
        public DisplayQuizSessionModel(DateTime endDate)
        {
            EndDate = endDate;
        }

        /// <summary>
        /// Gets or sets the quiz session end date.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
