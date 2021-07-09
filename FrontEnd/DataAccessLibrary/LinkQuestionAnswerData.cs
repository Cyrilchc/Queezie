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
    public class LinkQuestionAnswerData : ILinkQuestionAnswerData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public LinkQuestionAnswerData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataLinkQuestionAnswerModel>> GetLinkedAnswersApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/linkquestionanswers/question/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataLinkQuestionAnswerModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<DataLinkQuestionAnswerModel>> GetLinksApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/linkquestionanswers");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataLinkQuestionAnswerModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertLinkApi(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/linkquestionanswers", new StringContent(
                JsonConvert.SerializeObject(
                new
                {
                    questionId = dataLinkQuestionAnswerModel.QuestionId,
                    answerId = dataLinkQuestionAnswerModel.AnswerId
                }),
                Encoding.UTF8,
                "application/json"
                ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteLinkApi(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/linkquestionanswers/delete", new StringContent(
               JsonConvert.SerializeObject(
               new
               {
                   questionId = dataLinkQuestionAnswerModel.QuestionId,
                   answerId = dataLinkQuestionAnswerModel.AnswerId
               }),
               Encoding.UTF8,
               "application/json"
               ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get question answers.
        /// </summary>
        public Task<List<DataLinkQuestionAnswerModel>> GetLinkedAnswers(string id)
        {
            string sql = $"select * from linkquestionanswer where questionId={id}";
            return _db.LoadData<DataLinkQuestionAnswerModel, dynamic>(sql, new { });
        }

        public Task<List<DataLinkQuestionAnswerModel>> GetLinks()
        {
            string sql = "select * from linkquestionanswer";
            return _db.LoadData<DataLinkQuestionAnswerModel, dynamic>(sql, new { });
        }

        public Task InsertLink(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel)
        {
            string sql = @"insert into linkquestionanswer (questionId, answerId) values (@QuestionId, @AnswerId);";
            return _db.SaveData(sql, dataLinkQuestionAnswerModel);
        }

        public Task DeleteLink(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel)
        {
            string sql = @"delete from linkquestionanswer where questionId=@QuestionId and answerId=@AnswerId;";
            return _db.SaveData(sql, dataLinkQuestionAnswerModel);
        }
    }
}
