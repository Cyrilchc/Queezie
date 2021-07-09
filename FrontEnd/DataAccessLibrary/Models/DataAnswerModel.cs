using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class DataAnswerModel
    {
        /// <summary>
        /// Gets or Sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets the answer.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether the answer is correct or not.
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether the player has selected the answer or not.
        /// </summary>
        public bool PlayerAnswer { get; set; }
    }
}
