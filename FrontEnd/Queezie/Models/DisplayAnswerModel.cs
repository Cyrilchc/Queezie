using System.ComponentModel.DataAnnotations;

namespace Queezie.Models
{
    public class DisplayAnswerModel
    {
        public DisplayAnswerModel(string answer, bool type, string id)
        {
            Answer = answer;
            Type = type;
            Id = id;
        }

        public DisplayAnswerModel()
        {
        }

        /// <summary>
        /// Gets or Sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets the answer.
        /// </summary>
        [Display(Name = "Réponse")]
        [Required(ErrorMessage = "Veuillez entrer l'intitulé de la réponse que vous souhaitez ajouter")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether the answer is correct or not.
        /// </summary>
        [Display(Name = "La réponse est-elle correcte ?")]
        public bool Type { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether the player has selected the answer or not.
        /// </summary>
        public bool PlayerAnswer { get; set; }
    }
}
