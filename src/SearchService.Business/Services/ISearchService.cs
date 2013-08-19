namespace SearchService.Business.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using global::SearchService.Business.Entities;

    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        bool HasOrganization(string organizationId);
        [OperationContract]
        bool AddOrganization(string organizationId, string encryptionKey);

        [OperationContract]
        Task<bool> AddToSearch(string organizationId, string text, string encryptedResultTitle, string contentId, string contentType, bool isEncrypted);

        [OperationContract]
        Task<List<SearchItemDto>> Search(string organizationId, string searchKey);
    }
}