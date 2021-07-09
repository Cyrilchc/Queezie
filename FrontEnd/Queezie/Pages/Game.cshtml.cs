using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Queezie.Globals;
using Queezie.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queezie.Pages
{
    public class GameModel : PageModel
    {
        private readonly ISqlDataAccess _db;
        private readonly ILogger<GameModel> _logger;

        public GameModel(ISqlDataAccess db, ILogger<GameModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the selected Quiz.
        /// </summary>
        [BindProperty]
        public DisplayQuizModel SelectedQuiz { get; set; }

        /// <summary>
        /// Gets or sets the game question / answers object.
        /// </summary>
        [BindProperty]
        public DisplayGameQuestionAnswerModel DisplayGameQuestionAnswer { get; set; }

        /// <summary>
        /// Gets or sets the question type.
        /// </summary>

        public DisplayQuestionTypeModel DisplayQuestionTypeModel { get; set; }

        /// <summary>
        /// Gets or sets Quizs Proposed to the user to begin a game.
        /// </summary>
        public SelectList Quizs { get; set; }

        /// <summary>
        /// Gets the available quizes and display them in the dropdown list.
        /// </summary>
        public async Task OnGetAsync()
        {
            // No quiz is selected, we should display a list of quiz that the player can select
            // Getting all quizes
            List<DisplayQuizModel> displayQuizModels = await GetQuizs();

            // Binding them to the UI
            Quizs = new SelectList(displayQuizModels, "Id", "Quiz");
        }

        /// <summary>
        /// Display the question and its answers.
        /// </summary>
        /// <param name="id">The question's id</param>
        public async Task OnGetQuestionAsync(string id)
        {
            // ReBuilding SelectedQuiz...
            SelectedQuiz = new DisplayQuizModel { Id = id };

            // Quiz is selected, the question can be displayed
            // Getting Quiz's random question
            DisplayGameQuestionModel selectedQuestion = await GetRandomQuestionFromQuiz(SelectedQuiz.Id);

            // Getting Question's answers
            List<DisplayAnswerModel> questionsAnswers = await GetQuestionAnswers(selectedQuestion.Id);

            // Building the game question / answers object
            DisplayGameQuestionAnswer = new DisplayGameQuestionAnswerModel
            {
                Question = selectedQuestion,
                Answers = questionsAnswers,
            };

            DisplayViewData();
        }

        /// <summary>
        /// Process the user answer to the question.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Session.EndSession == default(DateTime) || SelectedQuiz == null)
                {
                    // Session doesn't exists, the player wants to start the game, setting the game parameters and display them
                    // Getting the quiz as we only have the Id
                    QuizData quizData = new QuizData(_db);
                    DataQuizModel dataQuizModel = await quizData.GetQuizByIdApi(SelectedQuiz.Id)
                        .ContinueWith((x) => { return x.Result[0]; });
                    SelectedQuiz = new DisplayQuizModel
                    {
                        Duration = dataQuizModel.Duration,
                        Id = dataQuizModel.Id,
                        Quiz = dataQuizModel.Quiz,
                    };

                    Session.EndSession = DateTime.Now.AddSeconds(SelectedQuiz.Duration);
                    Score.PlayerScore = 0;
                    return RedirectToPage("./game", "question", new {id = SelectedQuiz.Id});
                }
                else
                {
                    // The session exists, the player just answered a question
                    if (DateTime.Now > Session.EndSession)
                    {
                        // Session expired, Game finished
                        Session.EndSession = default(DateTime);
                        string tempScore = Score.PlayerScore.ToString();
                        Score.PlayerScore = 0;

                        // Display a result page where the user can see its score
                        return RedirectToPage("./resultpage", new {score = tempScore});
                    }
                    else
                    {
                        // The player still has time, checking its answer...
                        bool answersAreCorrect = true;
                        foreach (DisplayAnswerModel answer in DisplayGameQuestionAnswer.Answers)
                        {
                            if (answer.Type != answer.PlayerAnswer)
                            {
                                answersAreCorrect = false;
                                break;
                            }
                        }

                        if (answersAreCorrect)
                        {
                            Score.PlayerScore++;
                        }

                        return RedirectToPage("./game", "question", new {id = SelectedQuiz.Id});
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToPage("./game");
            }
        }

        /// <summary>
        /// Get all quizs.
        /// </summary>
        private async Task<List<DisplayQuizModel>> GetQuizs()
        {
            QuizData quizData = new QuizData(_db);
            List<DisplayQuizModel> quizModels = new List<DisplayQuizModel>();
            List<DataQuizModel> dataQuizModels = await quizData.GetQuizsApi();
            foreach (var dataQuizModel in dataQuizModels)
            {
                quizModels.Add(new DisplayQuizModel
                {
                    Id = dataQuizModel.Id,
                    Duration = dataQuizModel.Duration,
                    Quiz = dataQuizModel.Quiz,
                });
            }

            return quizModels;
        }

        /// <summary>
        /// Get a random question linked to the quiz.
        /// </summary>
        /// <param name="id">The quiz id.</param>
        /// <returns>A random question.</returns>
        private async Task<DisplayGameQuestionModel> GetRandomQuestionFromQuiz(string id)
        {
            Random random = new Random();

            // Database related helpers
            LinkQuizQuestionData linkQuizQuestionData = new LinkQuizQuestionData(_db);
            QuestionData questionData = new QuestionData(_db);
            QuestionTypeData questionTypeData = new QuestionTypeData(_db);

            // Get Quiz questions and select one randomly
            List<DataLinkQuizQuestionModel> dataLinkQuizQuestionModels = await linkQuizQuestionData.GetLinkedQuestionsApi(id);
            DataLinkQuizQuestionModel randomlySelectedlink = dataLinkQuizQuestionModels[random.Next(dataLinkQuizQuestionModels.Count)];
            DataQuestionModel randomlySelectedQuestion = await questionData.GetQuestionByIdApi(randomlySelectedlink.QuestionId).ContinueWith((x) => { return x.Result[0]; });

            // Get Question Type to get the real name
            DataQuestionTypeModel dataQuestionTypeModel = await questionTypeData.GetQuestionTypeByIdApi(randomlySelectedQuestion.QuestionTypeId).ContinueWith((x) => { return x.Result[0]; });

            // Result the translated object
            return new DisplayGameQuestionModel
            {
                Id = randomlySelectedQuestion.Id,
                Question = randomlySelectedQuestion.Question,
                QuestionType = dataQuestionTypeModel.QuestionType,
            };
        }

        /// <summary>
        /// Get the question's answers.
        /// </summary>
        /// <param name="id">The question id.</param>
        /// <returns>The list of question's answers.</returns>
        private async Task<List<DisplayAnswerModel>> GetQuestionAnswers(string id)
        {
            // Database related helpers
            LinkQuestionAnswerData linkQuestionAnswerData = new LinkQuestionAnswerData(_db);
            AnswerData answerData = new AnswerData(_db);

            // Get question's answers
            List<DataLinkQuestionAnswerModel> dataLinkQuestionAnswerModels = await linkQuestionAnswerData.GetLinkedAnswersApi(id);

            // Build the display object answers
            List<DisplayAnswerModel> displayAnswerModels = new List<DisplayAnswerModel>();
            foreach (var dataLinkQuestionAnswerModel in dataLinkQuestionAnswerModels)
            {
                DataAnswerModel dataAnswerModel = await answerData.GetAnswerByIdApi(dataLinkQuestionAnswerModel.AnswerId).ContinueWith((x) => { return x.Result[0]; });
                displayAnswerModels.Add(new DisplayAnswerModel
                {
                    Answer = dataAnswerModel.Answer,
                    Id = dataAnswerModel.Id,
                    PlayerAnswer = dataAnswerModel.PlayerAnswer,
                    Type = dataAnswerModel.Type,
                });
            }

            return displayAnswerModels;
        }

        /// <summary>
        /// Displays ViewData elements.
        /// </summary>
        private void DisplayViewData()
        {
            ViewData["Score"] = Score.PlayerScore;
            ViewData["ScoreDisplay"] = $"Score : {Score.PlayerScore}";
            ViewData["EndSession"] = $"Fin de la session : {Session.EndSession}";
        }
    }
}
