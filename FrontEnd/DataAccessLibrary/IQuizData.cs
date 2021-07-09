using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IQuizData
    {
        Task DeleteQuizApi(DataQuizModel quizModel);
        Task<List<DataQuizModel>> GetQuizByIdApi(string id);
        Task<List<DataQuizModel>> GetQuizsApi();
        Task InsertQuizApi(DataQuizModel quizModel);
    }
}