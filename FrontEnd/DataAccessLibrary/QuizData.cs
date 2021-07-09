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
    public class QuizData : IQuizData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public QuizData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataQuizModel>> GetQuizsApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/quizs");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuizModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<DataQuizModel>> GetQuizByIdApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/quizs/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuizModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertQuizApi(DataQuizModel quizModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/quizs", new StringContent(
                    JsonConvert.SerializeObject(
                    new
                    {
                        quiz = quizModel.Quiz,
                        duration = quizModel.Duration
                    }),
                    Encoding.UTF8,
                    "application/json"
                    ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteQuizApi(DataQuizModel quizModel)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{Configuration["Api:RootUrl"]}/quizs/{quizModel.Id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public Task<List<DataQuizModel>> GetQuizs()
        {
            string sql = "select * from quiz";
            return _db.LoadData<DataQuizModel, dynamic>(sql, new { });
        }

        public Task<List<DataQuizModel>> GetQuizById(string id)
        {
            string sql = $"select * from quiz where id={id}";
            return _db.LoadData<DataQuizModel, dynamic>(sql, new { });
        }

        public Task InsertQuiz(DataQuizModel quizModel)
        {
            string sql = @"insert into quiz (quiz, duration) values(@Quiz, @Duration);";
            return _db.SaveData(sql, quizModel);
        }

        public Task DeleteQuiz(DataQuizModel quizModel)
        {
            string sql = @"delete from quiz where id=@Id;";
            return _db.SaveData(sql, quizModel);
        }
    }
}
