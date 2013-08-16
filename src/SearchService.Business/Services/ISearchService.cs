namespace SearchService.Business.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using SearchService.Business.Entities;

    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        bool HasOrganization(string organizationId);
        [OperationContract]
        bool AddOrganization(string organizationId, string encryptionKey);

        [OperationContract]
        bool AddToSearch(string organizationId, string encryptedText, string contentId, string contentType);
        
        [OperationContract]
        Task<List<SearchItemDto>> Search(string organizationId, string searchKey);
    }
}