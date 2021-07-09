using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ILinkQuestionAnswerData
    {
        Task DeleteLinkApi(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel);
        Task<List<DataLinkQuestionAnswerModel>> GetLinkedAnswersApi(string id);
        Task<List<DataLinkQuestionAnswerModel>> GetLinksApi();
        Task InsertLinkApi(DataLinkQuestionAnswerModel dataLinkQuestionAnswerModel);
    }
}