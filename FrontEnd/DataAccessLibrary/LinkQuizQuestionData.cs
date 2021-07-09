using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class LinkQuizQuestionData : ILinkQuizQuestionData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public LinkQuizQuestionData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }
        public async Task<List<DataLinkQuizQuestionModel>> GetLinkedQuestionsApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/linkquizquestions/quiz/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataLinkQuizQuestionModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<DataLinkQuizQuestionModel>> GetLinksApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/linkquizquestions");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataLinkQuizQuestionModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertLinkApi(DataLinkQuizQuestionModel dataLinkQuizQuestionModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/linkquizquestions", new StringContent(
                      JsonConvert.SerializeObject(
                      new
                      {
                          quizId = dataLinkQuizQuestionModel.QuizId,
                          questionId = dataLinkQuizQuestionModel.QuestionId
                      }),
                      Encoding.UTF8,
                      "application/json"
                      ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteQuestionApi(DataLinkQuizQuestionModel dataLinkQuizQuestionModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/linkquizquestions/delete", new StringContent(
             JsonConvert.SerializeObject(
             new
             {
                 quizId = dataLinkQuizQuestionModel.QuizId,
                 questionId = dataLinkQuizQuestionModel.QuestionId
             }),
             Encoding.UTF8,
             "application/json"
             ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get quiz questions.
        /// </summary>
        public Task<List<DataLinkQuizQuestionModel>> GetLinkedQuestions(string id)
        {
            string sql = $"select * from linkquizquestion where quizId={id}";
            return _db.LoadData<DataLinkQuizQuestionModel, dynamic>(sql, new { });
        }

        public Task<List<DataLinkQuizQuestionModel>> GetLinks()
        {
            string sql = "select * from linkquizquestion";
            return _db.LoadData<DataLinkQuizQuestionModel, dynamic>(sql, new { });
        }

        public Task InsertLink(DataLinkQuizQuestionModel dataLinkQuizQuestionModel)
        {
            string sql = @"insert into linkquizquestion (quizId, questionId) values (@QuizId, @QuestionId);";
            return _db.SaveData(sql, dataLinkQuizQuestionModel);
        }

        public Task DeleteQuestion(DataLinkQuizQuestionModel dataLinkQuizQuestionModel)
        {
            string sql = @"delete from linkquizquestion where quizId=@QuizId and questionId=@QuestionId;";
            return _db.SaveData(sql, dataLinkQuizQuestionModel);
        }
    }
}
