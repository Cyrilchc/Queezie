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
    public class QuestionData : IQuestionData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public QuestionData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataQuestionModel>> GetQuestionsApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/questions");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuestionModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
        public async Task<List<DataQuestionModel>> GetQuestionByIdApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/questions/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuestionModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertQuestionApi(DataQuestionModel questionModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/questions", new StringContent(
                    JsonConvert.SerializeObject(
                    new
                    {
                        question = questionModel.Question,
                        questionTypeId = questionModel.QuestionTypeId,
                        domainId = questionModel.DomainId
                    }),
                    Encoding.UTF8,
                    "application/json"
                    ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteQuestionApi(DataQuestionModel questionModel)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{Configuration["Api:RootUrl"]}/questions/{questionModel.Id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public Task<List<DataQuestionModel>> GetQuestions()
        {
            string sql = "select * from question";
            return _db.LoadData<DataQuestionModel, dynamic>(sql, new { });
        }

        public Task<List<DataQuestionModel>> GetQuestionById(string id)
        {
            string sql = $"select * from question where id={id}";
            return _db.LoadData<DataQuestionModel, dynamic>(sql, new { });
        }

        public Task InsertQuestion(DataQuestionModel questionModel)
        {
            string sql = @"insert into question (question, questionTypeId, domainId) values(@Question, @QuestionTypeId, @DomainId);";
            return _db.SaveData(sql, questionModel);
        }

        public Task DeleteQuestion(DataQuestionModel questionModel)
        {
            string sql = @"delete from question where id=@Id;";
            return _db.SaveData(sql, questionModel);
        }
    }
}
