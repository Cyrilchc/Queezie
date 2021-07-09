using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Queezie.Pages
{
    public class ResultPageModel : PageModel
    {
        [BindProperty]
        public string ResultScore { get; set; }

        /// <summary>
        /// Gets the score and displays it.
        /// </summary>
        /// <param name="score">The score.</param>
        public void OnGet(string score)
        {
            ResultScore = score;
        }
    }
}
