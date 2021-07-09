using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IQuestionTypeData
    {
        Task<List<DataQuestionTypeModel>> GetQuestionTypesApi();
        Task<List<DataQuestionTypeModel>> GetQuestionTypeByIdApi(string id);
    }
}