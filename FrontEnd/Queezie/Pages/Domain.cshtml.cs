using DataAccessLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Queezie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class DomainModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public DomainModel(ISqlDataAccess db)
        {
            _db = db;
        }

        [BindProperty]
        public DisplayDomainModel DisplayDomain { get; set; }

        [BindProperty]
        public List<DisplayDomainModel> Domains { get; set; }

        /// <summary>
        /// Gets the domains and display them.
        /// </summary>
        public async Task OnGetAsync()
        {
            List<DisplayDomainModel> domainModels = new List<DisplayDomainModel>();
            DomainData domainData = new DomainData(_db);
            List<DataDomainModel> dataAccessDomainModels = await domainData.GetDomainsApi();
            foreach (DataDomainModel dataAccessDomainModel in dataAccessDomainModels)
            {
                domainModels.Add(new DisplayDomainModel
                {
                    Domain = dataAccessDomainModel.Domain,
                    Id = dataAccessDomainModel.Id,
                });
            }

            Domains = domainModels;
        }

        /// <summary>
        /// Creates a new domain.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DomainData domainData = new DomainData(_db);
            DataDomainModel newDomainModel = new DataDomainModel
            {
                Domain = DisplayDomain.Domain,
                Id = DisplayDomain.Id,
            };

            await domainData.InsertDomainApi(newDomainModel);
            return RedirectToPage("./domain");
        }

        /// <summary>
        /// Deletes the domain.
        /// </summary>
        /// <param name="id">The domain's id.</param>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            DomainData domainData = new DomainData(_db);
            DataDomainModel newDomainModel = new DataDomainModel
            {
                Domain = DisplayDomain.Domain,
                Id = DisplayDomain.Id,
            };

            await domainData.DeleteDomainApi(newDomainModel);
            return RedirectToPage("./domain");
        }
    }
}
