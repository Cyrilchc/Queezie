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
    public class AnswerData : IAnswerData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public AnswerData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataAnswerModel>> GetAnswersApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/answers");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataAnswerModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<DataAnswerModel>> GetAnswerByIdApi(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/answers/{id}");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataAnswerModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertAnswerApi(DataAnswerModel domainModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/answers", new StringContent(
                JsonConvert.SerializeObject(
                new
                {
                    answer = domainModel.Answer,
                    type = domainModel.Type ? "1" : "0"
                }),
                Encoding.UTF8,
                "application/json"
                ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            // L'api peut retourner l'objet nouvellement créé, dans ce front end, on en a pas besoin
            //if (response.IsSuccessStatusCode)
            //{
            //    return await response.Content.ReadAsStringAsync();
            //}
            //else
            //{
            //    throw new Exception(await response.Content.ReadAsStringAsync());
            //}
        }

        public async Task DeleteAnswerApi(DataAnswerModel domainModel)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{Configuration["Api:RootUrl"]}/answers/{domainModel.Id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public Task<List<DataAnswerModel>> GetAnswers()
        {
            string sql = "select * from answer";
            return _db.LoadData<DataAnswerModel, dynamic>(sql, new { });
        }

        public Task<List<DataAnswerModel>> GetAnswerById(string id)
        {
            string sql = $"select * from answer where id={id}";
            return _db.LoadData<DataAnswerModel, dynamic>(sql, new { });
        }

        public Task InsertAnswer(DataAnswerModel domainModel)
        {
            string sql = @"insert into answer (answer, type) values(@Answer, @Type);";
            return _db.SaveData(sql, domainModel);
        }

        public Task DeleteAnswer(DataAnswerModel domainModel)
        {
            string sql = @"delete from answer where id=@Id;";
            return _db.SaveData(sql, domainModel);
        }
    }
}
