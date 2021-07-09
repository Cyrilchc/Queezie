using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class QuestionTypeData : IQuestionTypeData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public QuestionTypeData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataQuestionTypeModel>> GetQuestionTypesApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/questionTypes");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuestionTypeModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<DataQuestionTypeModel>> GetQuestionTypeByIdApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/questionTypes/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataQuestionTypeModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public Task<List<DataQuestionTypeModel>> GetQuestionTypes()
        {
            string sql = "select * from questiontype";
            return _db.LoadData<DataQuestionTypeModel, dynamic>(sql, new { });
        }

        public Task<List<DataQuestionTypeModel>> GetQuestionTypeById(string id)
        {
            string sql = $"select * from questiontype where id={id}";
            return _db.LoadData<DataQuestionTypeModel, dynamic>(sql, new { });
        }
    }
}
