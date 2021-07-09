using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Queezie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class QuizModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public QuizModel(ISqlDataAccess db)
        {
            _db = db;
        }

        [BindProperty]
        public DisplayQuizModel DisplayQuiz { get; set; }

        [BindProperty]
        public List<DisplayQuizModel> Quizs { get; set; }

        /// <summary>
        /// Gets all quizes and display them.
        /// </summary>
        public async Task OnGetAsync()
        {
            List<DisplayQuizModel> quizModels = new List<DisplayQuizModel>();
            QuizData quizData = new QuizData(_db);
            List<DataQuizModel> dataAccessQuizModels = await quizData.GetQuizsApi();
            foreach (DataQuizModel dataAccessQuizModel in dataAccessQuizModels)
            {
                quizModels.Add(new DisplayQuizModel
                {
                    Id = dataAccessQuizModel.Id,
                    Duration = dataAccessQuizModel.Duration,
                    Quiz = dataAccessQuizModel.Quiz,
                });
            }

            Quizs = quizModels;
        }

        /// <summary>
        /// Adds a new quiz.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            QuizData quizData = new QuizData(_db);
            DataQuizModel newQuizModel = new DataQuizModel
            {
                Duration = DisplayQuiz.Duration,
                Id = DisplayQuiz.Id,
                Quiz = DisplayQuiz.Quiz,
            };

            await quizData.InsertQuizApi(newQuizModel);
            return RedirectToPage("./quiz");
        }

        /// <summary>
        /// Deletes the quiz.
        /// </summary>
        /// <param name="id">The quiz's id.</param>
        /// <returns>Refresh page.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            QuizData quizData = new QuizData(_db);
            DataQuizModel newQuizModel = new DataQuizModel
            {
                Duration = DisplayQuiz.Duration,
                Id = DisplayQuiz.Id,
                Quiz = DisplayQuiz.Quiz,
            };

            await quizData.DeleteQuizApi(newQuizModel);
            return RedirectToPage("./quiz");
        }

        /// <summary>
        /// Redirect to the quiz's questions.
        /// </summary>
        /// <param name="id">The quiz's id.</param>
        /// <returns>Redirects.</returns>
        public IActionResult OnPostQuestions(int id)
        {
            return RedirectToPage("./linkquiztoquestion", new { id = id });
        }
    }
}
