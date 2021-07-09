using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Queezie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class QuestionModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public QuestionModel(ISqlDataAccess db)
        {
            _db = db;

        }

        [BindProperty]
        public DisplayQuestionModel DisplayQuestion { get; set; }

        [BindProperty]
        public List<DisplayQuestionModel> Questions { get; set; }

        public SelectList Domains { get; set; }

        public SelectList QuestionTypes { get; set; }

        /// <summary>
        /// Gets all questions and display them.
        /// </summary>
        public async Task OnGetAsync()
        {
            Domains = new SelectList(await GetDomains(), "Id", "Domain");
            QuestionTypes = new SelectList(await GetQuestionTypes(), "Id", "QuestionType");
            List<DisplayQuestionModel> questionModels = new List<DisplayQuestionModel>();
            QuestionData questionData = new QuestionData(_db);
            List<DataQuestionModel> dataAccessQuestionModels = await questionData.GetQuestionsApi();
            foreach (DataQuestionModel dataAccessQuestionModel in dataAccessQuestionModels)
            {
                questionModels.Add(new DisplayQuestionModel
                {
                    Question = dataAccessQuestionModel.Question,
                    Id = dataAccessQuestionModel.Id,
                    DomainId = dataAccessQuestionModel.DomainId,
                    QuestionTypeId = dataAccessQuestionModel.QuestionTypeId,
                });
            }

            Questions = questionModels;
        }

        /// <summary>
        /// Adds a new question.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            QuestionData questionData = new QuestionData(_db);
            DataQuestionModel newQuestionModel = new DataQuestionModel
            {
                Question = DisplayQuestion.Question,
                DomainId = DisplayQuestion.DomainId,
                Id = DisplayQuestion.Id,
                QuestionTypeId = DisplayQuestion.QuestionTypeId,
            };

            await questionData.InsertQuestionApi(newQuestionModel);
            return RedirectToPage("./question");
        }

        /// <summary>
        /// Deletes the question.
        /// </summary>
        /// <param name="id">Question's id.</param>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            QuestionData questionData = new QuestionData(_db);
            DataQuestionModel newQuestionModel = new DataQuestionModel
            {
                Question = DisplayQuestion.Question,
                DomainId = DisplayQuestion.DomainId,
                Id = DisplayQuestion.Id,
                QuestionTypeId = DisplayQuestion.QuestionTypeId,
            };

            await questionData.DeleteQuestionApi(newQuestionModel);
            return RedirectToPage("./question");
        }

        /// <summary>
        /// Redirects to the question's answers page.
        /// </summary>
        /// <param name="id">The question's id.</param>
        /// <returns>Redirects.</returns>
        public IActionResult OnPostAnswers(int id)
        {
            return RedirectToPage("./linkquestiontoanswer", new { id = id });
        }

        /// <summary>
        /// Get all questionTypes.
        /// </summary>
        private async Task<List<DisplayQuestionTypeModel>> GetQuestionTypes()
        {
            QuestionTypeData questionTypeData = new QuestionTypeData(_db);
            List<DisplayQuestionTypeModel> questionTypeModels = new List<DisplayQuestionTypeModel>();
            List<DataQuestionTypeModel> dataQuestionTypeModels = await questionTypeData.GetQuestionTypesApi();
            foreach (var dataQuestionTypeModel in dataQuestionTypeModels)
            {
                questionTypeModels.Add(new DisplayQuestionTypeModel
                {
                    QuestionType = dataQuestionTypeModel.QuestionType,
                    Id = dataQuestionTypeModel.Id,
                });
            }

            return questionTypeModels;
        }

        /// <summary>
        /// Get all domains.
        /// </summary>
        private async Task<List<DisplayDomainModel>> GetDomains()
        {
            DomainData domainData = new DomainData(_db);
            List<DisplayDomainModel> domainModels = new List<DisplayDomainModel>();
            List<DataDomainModel> dataDomainModels = await domainData.GetDomainsApi();
            foreach (var dataDomainModel in dataDomainModels)
            {
                domainModels.Add(new DisplayDomainModel
                {
                    Domain = dataDomainModel.Domain,
                    Id = dataDomainModel.Id,
                });
            }

            return domainModels;
        }
    }
}
