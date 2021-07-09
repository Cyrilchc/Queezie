using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Queezie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class AnswerModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public AnswerModel(ISqlDataAccess db)
        {
            _db = db;
        }

        [BindProperty]
        public DisplayAnswerModel DisplayAnswer { get; set; }

        [BindProperty]
        public List<DisplayAnswerModel> Answers { get; set; }

        /// <summary>
        /// Gets the answers and display them.
        /// </summary>
        public async Task OnGetAsync()
        {
            List<DisplayAnswerModel> answerModels = new List<DisplayAnswerModel>();
            AnswerData answerData = new AnswerData(_db);
            List<DataAnswerModel> dataAccessAnswerModels = await answerData.GetAnswersApi();
            foreach (DataAnswerModel dataAccessAnswerModel in dataAccessAnswerModels)
            {
                answerModels.Add(new DisplayAnswerModel
                {
                    Answer = dataAccessAnswerModel.Answer,
                    Id = dataAccessAnswerModel.Id,
                    PlayerAnswer = dataAccessAnswerModel.PlayerAnswer,
                    Type = dataAccessAnswerModel.Type,
                });
            }

            Answers = answerModels;
        }

        /// <summary>
        /// Inserts a new answer.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AnswerData answerData = new AnswerData(_db);
            DataAnswerModel newAnswerModel = new DataAnswerModel
            {
                Answer = DisplayAnswer.Answer,
                Id = DisplayAnswer.Id,
                PlayerAnswer = DisplayAnswer.PlayerAnswer,
                Type = DisplayAnswer.Type,
            };

            await answerData.InsertAnswerApi(newAnswerModel);
            return RedirectToPage("./answer");
        }

        /// <summary>
        /// Deletes the answer.
        /// </summary>
        /// <param name="id">The answer's id.</param>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            AnswerData answerData = new AnswerData(_db);
            DataAnswerModel newAnswerModel = new DataAnswerModel
            {
                Answer = DisplayAnswer.Answer,
                Id = DisplayAnswer.Id,
                PlayerAnswer = DisplayAnswer.PlayerAnswer,
                Type = DisplayAnswer.Type,
            };

            await answerData.DeleteAnswerApi(newAnswerModel);
            return RedirectToPage("./answer");
        }
    }
}
