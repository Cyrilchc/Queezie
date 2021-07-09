using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class DataQuestionModel
    {
        public string Id { get; set; }

        public string Question { get; set; }

        public string DomainId { get; set; }

        public string QuestionTypeId { get; set; }
    }
}
