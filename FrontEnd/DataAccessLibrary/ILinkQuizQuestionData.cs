using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ILinkQuizQuestionData
    {
        Task DeleteQuestionApi(DataLinkQuizQuestionModel dataLinkQuizQuestionModel);
        Task<List<DataLinkQuizQuestionModel>> GetLinkedQuestionsApi(string id);
        Task<List<DataLinkQuizQuestionModel>> GetLinksApi();
        Task InsertLinkApi(DataLinkQuizQuestionModel dataLinkQuizQuestionModel);
    }
}