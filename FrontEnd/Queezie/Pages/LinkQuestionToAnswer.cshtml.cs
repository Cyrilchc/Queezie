using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Queezie.Models;

namespace Queezie.Pages
{
    public class LinkQuestionToAnswerModel : PageModel
    {
        private readonly ISqlDataAccess _db;

        public LinkQuestionToAnswerModel(ISqlDataAccess db)
        {
            _db = db;
        }

        [BindProperty]
        public DisplayLinkQuestionAnswerModel DisplayLink { get; set; }

        [BindProperty]
        public List<DisplayLinkQuestionAnswerModel> DisplayLinks { get; set; }

        public SelectList Answers { get; set; }

        public List<DisplayAnswerModel> DisplayAnswers { get; set; }

        public DisplayQuestionModel DisplayQuestionModel { get; set; }

        public DisplayQuestionTypeModel DisplayQuestionTypeModel { get; set; }

        /// <summary>
        /// Get all the question's linked answers and display them.
        /// </summary>
        /// <param name="id">The question id.</param>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            DisplayAnswers = new List<DisplayAnswerModel>();

            // Getting the current question
            QuestionData questionData = new QuestionData(_db);
            var dataQuestionModels = await questionData.GetQuestionByIdApi(id);
            DisplayQuestionModel = new DisplayQuestionModel
            {
                DomainId = dataQuestionModels[0].DomainId,
                Id = dataQuestionModels[0].Id,
                Question = dataQuestionModels[0].Question,
                QuestionTypeId = dataQuestionModels[0].QuestionTypeId,
            };

            // Getting Question type
            QuestionTypeData questionTypeData = new QuestionTypeData(_db);
            var dataQuestionTypeModels =
                await questionTypeData.GetQuestionTypeByIdApi(DisplayQuestionModel.QuestionTypeId);
            DisplayQuestionTypeModel = new DisplayQuestionTypeModel
            {
                Id = dataQuestionTypeModels[0].Id,
                QuestionType = dataQuestionTypeModels[0].QuestionType,
            };

            // Getting links
            List<DisplayLinkQuestionAnswerModel> displayLinkQuestionAnswerModels = new List<DisplayLinkQuestionAnswerModel>();
            LinkQuestionAnswerData linkQuestionAnswerData = new LinkQuestionAnswerData(_db);
            List<DataLinkQuestionAnswerModel> dataAccessLinkQuestionAnswerModels = await linkQuestionAnswerData.GetLinkedAnswersApi(id);
            foreach (DataLinkQuestionAnswerModel dataAccessLinkQuestionAnswerModel in dataAccessLinkQuestionAnswerModels)
            {
                displayLinkQuestionAnswerModels.Add(new DisplayLinkQuestionAnswerModel
                {
                    AnswerId = dataAccessLinkQuestionAnswerModel.AnswerId,
                    Id = dataAccessLinkQuestionAnswerModel.Id,
                    QuestionId = dataAccessLinkQuestionAnswerModel.QuestionId,
                });
            }

            DisplayLinks = displayLinkQuestionAnswerModels;

            // Getting answers for the dropdown list.
            List<DisplayAnswerModel> answers = await GetAnswers();
            Answers =
            new SelectList(
                answers.Select(x => new
                {
                    Id = x.Id,
                    Answer = $"{x.Answer} ({(x.Type == true ? "Réponse correcte" : "Réponse incorrecte")})",
                }),
                "Id",
                "Answer");

            return Page();
        }

        /// <summary>
        /// Add a new answer to the question.
        /// </summary>
        /// <returns>Refresh the page.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LinkQuestionAnswerData linkQuestionAnswerData = new LinkQuestionAnswerData(_db);
            DataLinkQuestionAnswerModel newLinkQuestionAnswerModel = new DataLinkQuestionAnswerModel
            {
                AnswerId = DisplayLink.AnswerId,
                Id = DisplayLink.Id,
                QuestionId = DisplayLink.QuestionId,
            };

            // Getting the question type
            QuestionData questionData = new QuestionData(_db);
            var dataQuestionModel = await questionData.GetQuestionByIdApi(newLinkQuestionAnswerModel.QuestionId);
            QuestionTypeData questionTypeData = new QuestionTypeData(_db);
            var dataQuestionTypeModel = await questionTypeData.GetQuestionTypeByIdApi(dataQuestionModel[0].QuestionTypeId);

            // Getting the question answers
            List<DisplayAnswerModel> questionAnswers = await GetQuestionAnswers(newLinkQuestionAnswerModel.QuestionId);

            // Getting the user answer
            AnswerData answerData = new AnswerData(_db);
            var dataAnswerModel = await answerData.GetAnswerByIdApi(newLinkQuestionAnswerModel.AnswerId);
            if (dataAnswerModel[0].Type == true)
            {
                // If the answer to add is correct and it already exists a correct response in simple and boolean types, we shouldn't add the answer
                if ((dataQuestionTypeModel[0].QuestionType == "Simple" || dataQuestionTypeModel[0].QuestionType == "Boolean") &&
                    questionAnswers.Any(x => x.Type == true))
                {
                    return RedirectToPage("./linkquestiontoanswer", new { id = DisplayLink.QuestionId });
                }
            }

            await linkQuestionAnswerData.InsertLinkApi(newLinkQuestionAnswerModel);
            return RedirectToPage("./linkquestiontoanswer", new { id = DisplayLink.QuestionId });
        }

        /// <summary>
        /// Remove the question's answer.
        /// </summary>
        /// <param name="id">The answer's id.</param>
        /// <returns>Refresh page</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            LinkQuestionAnswerData linkQuestionAnswerData = new LinkQuestionAnswerData(_db);
            DataLinkQuestionAnswerModel newDataLinkQuestionAnswerModel = new DataLinkQuestionAnswerModel
            {
                Id = string.Empty,
                QuestionId = DisplayLink.QuestionId,
                AnswerId = id.ToString(),
            };

            await linkQuestionAnswerData.DeleteLinkApi(newDataLinkQuestionAnswerModel);
            return RedirectToPage("./linkquestiontoanswer", new { id = DisplayLink.QuestionId });
        }

        /// <summary>
        /// Get question's answers.
        /// </summary>
        /// <returns>A List of DisplayAnswerModel.</returns>
        public async Task<List<DisplayAnswerModel>> GetAnswers()
        {
            AnswerData answerData = new AnswerData(_db);
            List<DisplayAnswerModel> answerModels = new List<DisplayAnswerModel>();
            List<DataAnswerModel> dataAnswerModels = await answerData.GetAnswersApi();
            foreach (var dataAnswerModel in dataAnswerModels)
            {
                DisplayAnswerModel newDisplayAnswerModel = new DisplayAnswerModel
                {
                    Answer = dataAnswerModel.Answer,
                    Id = dataAnswerModel.Id,
                    PlayerAnswer = dataAnswerModel.PlayerAnswer,
                    Type = dataAnswerModel.Type,
                };

                if (!DisplayLinks.Any(x => x.AnswerId == dataAnswerModel.Id))
                {
                    // Unadded questions are added to the dropdown
                    answerModels.Add(newDisplayAnswerModel);
                }
                else
                {
                    // Already linked questions are displayed in the table
                    DisplayAnswers.Add(newDisplayAnswerModel);
                }
            }

            return answerModels;
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

    }
}
