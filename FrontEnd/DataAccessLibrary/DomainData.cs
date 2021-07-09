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
    public class DomainData : IDomainData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot Configuration;
        private readonly HttpClient _httpClient;

        public DomainData(ISqlDataAccess db)
        {
            _db = db;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Configuration = new ConfigurationBuilder()
                     .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public async Task<List<DataDomainModel>> GetDomainsApi()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{Configuration["Api:RootUrl"]}/domains");
            if (response.IsSuccessStatusCode)
            {
                string datareceived = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DataDomainModel>>(datareceived);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task InsertDomainApi(DataDomainModel domainModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{Configuration["Api:RootUrl"]}/domains", new StringContent(
                JsonConvert.SerializeObject(
                   new
                   {
                       domain = domainModel.Domain,
                   }),
                Encoding.UTF8,
                "application/json"
                ));

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteDomainApi(DataDomainModel domainModel)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{Configuration["Api:RootUrl"]}/domains/{domainModel.Id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }


        public Task<List<DataDomainModel>> GetDomains()
        {
            string sql = "select * from domain";
            return _db.LoadData<DataDomainModel, dynamic>(sql, new { });
        }

        public Task InsertDomain(DataDomainModel domainModel)
        {
            string sql = @"insert into domain (domain) values(@Domain);";
            return _db.SaveData(sql, domainModel);
        }

        public Task DeleteDomain(DataDomainModel domainModel)
        {
            string sql = @"delete from domain where id=@Id;";
            return _db.SaveData(sql, domainModel);
        }
    }
}
