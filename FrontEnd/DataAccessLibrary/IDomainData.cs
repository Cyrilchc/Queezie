using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IDomainData
    {
        Task DeleteDomainApi(DataDomainModel domainModel);
        Task<List<DataDomainModel>> GetDomainsApi();
        Task InsertDomainApi(DataDomainModel domainModel);
    }
}