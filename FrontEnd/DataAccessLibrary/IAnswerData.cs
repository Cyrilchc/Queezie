using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IAnswerData
    {
        Task DeleteAnswerApi(DataAnswerModel domainModel);
        Task<List<DataAnswerModel>> GetAnswersApi();
        Task<List<DataAnswerModel>> GetAnswerByIdApi(string id);
        Task InsertAnswerApi(DataAnswerModel domainModel);
    }
}