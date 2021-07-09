using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Queezie.Models
{
    public class DisplayDomainModel
    {
        public DisplayDomainModel()
        {
        }

        public DisplayDomainModel(string id, string domain)
        {
            Id = id;
            Domain = domain;
        }

        public string Id { get; set; }

        [Display(Name = "Domaine")]
        [Required(ErrorMessage = "Veuillez entrer le nom du domaine que vous souhaitez ajouter")]
        public string Domain { get; set; }
    }
}
