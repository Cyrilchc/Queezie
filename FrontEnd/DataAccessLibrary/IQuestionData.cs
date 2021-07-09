using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IQuestionData
    {
        Task DeleteQuestionApi(DataQuestionModel questionModel);
        Task<List<DataQuestionModel>> GetQuestionsApi();
        Task<List<DataQuestionModel>> GetQuestionByIdApi(string id);
        Task InsertQuestionApi(DataQuestionModel questionModel);
    }
}