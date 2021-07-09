using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Queezie.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class LinkQuizToQuestionModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public LinkQuizToQuestionModel(ISqlDataAccess db)
        {
            _db = db;
        }

        [BindProperty]
        public DisplayLinkQuizQuestionModel DisplayLink { get; set; }

        [BindProperty]
        public List<DisplayLinkQuizQuestionModel> DisplayLinks { get; set; }

        public SelectList Questions { get; set; }

        public List<DisplayQuestionModel> DisplayQuestions { get; set; }

        public DisplayQuizModel DisplayQuizModel { get; set; }

        /// <summary>
        /// Get the quiz's questions.
        /// </summary>
        /// <param name="id">The quiz's id.</param>
        public async Task OnGetAsync(string id)
        {
            DisplayQuestions = new List<DisplayQuestionModel>();

            // Getting the current quiz
            QuizData quizData = new QuizData(_db);
            var dataQuizModels = await quizData.GetQuizByIdApi(id);
            DisplayQuizModel = new DisplayQuizModel
            {
                Duration = dataQuizModels[0].Duration,
                Id = dataQuizModels[0].Id,
                Quiz = dataQuizModels[0].Quiz,
            };

            // Getting links
            List<DisplayLinkQuizQuestionModel> displayLinkQuizQuestionModels = new List<DisplayLinkQuizQuestionModel>();
            LinkQuizQuestionData linkQuizQuestionData = new LinkQuizQuestionData(_db);
            List<DataLinkQuizQuestionModel> dataAccessLinkQuizQuestionModels = await linkQuizQuestionData.GetLinkedQuestionsApi(id);
            foreach (DataLinkQuizQuestionModel dataAccessLinkQuizQuestionModel in dataAccessLinkQuizQuestionModels)
            {
                displayLinkQuizQuestionModels.Add(new DisplayLinkQuizQuestionModel
                {
                    Id = dataAccessLinkQuizQuestionModel.Id,
                    QuestionId = dataAccessLinkQuizQuestionModel.QuestionId,
                    QuizId = dataAccessLinkQuizQuestionModel.QuizId,
                });
            }

            DisplayLinks = displayLinkQuizQuestionModels;

            // Gettings questions for the dropdown list
            Questions = new SelectList(await GetQuestions(), "Id", "Question");
        }

        /// <summary>
        /// Adding a new question to the quiz.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LinkQuizQuestionData linkQuizQuestionData = new LinkQuizQuestionData(_db);
            DataLinkQuizQuestionModel newLinkQuizQuestionModel = new DataLinkQuizQuestionModel
            {
                Id = DisplayLink.Id,
                QuestionId = DisplayLink.QuestionId,
                QuizId = DisplayLink.QuizId,
            };

            await linkQuizQuestionData.InsertLinkApi(newLinkQuizQuestionModel);
            return RedirectToPage("./linkquiztoquestion", new { id = DisplayLink.QuizId });
        }

        /// <summary>
        /// Remove the question of the quiz.
        /// </summary>
        /// <param name="id">The question's id.</param>
        /// <returns>Refresh the page</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            LinkQuizQuestionData linkQuizQuestionData = new LinkQuizQuestionData(_db);
            DataLinkQuizQuestionModel newDataLinkQuizQuestionModel = new DataLinkQuizQuestionModel
            {
                Id = string.Empty,
                QuestionId = id.ToString(),
                QuizId = DisplayLink.QuizId,
            };

            await linkQuizQuestionData.DeleteQuestionApi(newDataLinkQuizQuestionModel);
            return RedirectToPage("./linkquiztoquestion", new { id = DisplayLink.QuizId });
        }

        /// <summary>
        /// Get quiz's questions.
        /// </summary>
        /// <returns>A List of DisplayQuestionModel.</returns>
        public async Task<List<DisplayQuestionModel>> GetQuestions()
        {
            QuestionData questionData = new QuestionData(_db);
            List<DisplayQuestionModel> questionModels = new List<DisplayQuestionModel>();
            List<DataQuestionModel> dataQuestionModels = await questionData.GetQuestionsApi();
            foreach (var dataQuestionModel in dataQuestionModels)
            {
                DisplayQuestionModel newDisplayQuestionModel = new DisplayQuestionModel
                {
                    DomainId = dataQuestionModel.DomainId,
                    Id = dataQuestionModel.Id,
                    Question = dataQuestionModel.Question,
                    QuestionTypeId = dataQuestionModel.QuestionTypeId,
                };

                if (!DisplayLinks.Any(x => x.QuestionId == dataQuestionModel.Id))
                {
                    // Les questions non ajoutées sont ajoutées dans la liste déroulante
                    questionModels.Add(newDisplayQuestionModel);
                }
                else
                {
                    // Les questions déjà liées sont affichées dans le tableau
                    DisplayQuestions.Add(newDisplayQuestionModel);
                }
            }

            return questionModels;
        }
    }
}
