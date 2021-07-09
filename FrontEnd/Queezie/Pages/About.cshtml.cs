using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Queezie.Pages
{
    public class AboutModel : PageModel
    {
        public string MarkDown { get; set; }

        public void OnGet()
        {
        }
    }
}
