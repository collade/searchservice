namespace SearchService.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using global::SearchService.Business.Entities;
    using global::SearchService.Business.Repos;

    public class SearchService : ISearchService
    {
        private readonly IEntityRepository<Organization> organizationRepo;

        public SearchService(IEntityRepository<Organization> organizationRepo)
        {
            this.organizationRepo = organizationRepo;
        }

        public bool HasOrganization(string organizationId)
        {
            return organizationRepo.AsQueryable().Any(x => x.OrganizationId == organizationId);
        }

        public bool AddOrganization(string organizationId, string encryptionKey)
        {
            if (string.IsNullOrEmpty(organizationId))
            {
                return false;
            }

            if (HasOrganization(organizationId))
            {
                return false;
            }

            var result = organizationRepo.Add(new Organization { CreatedBy = "System", UpdatedBy = "System", OrganizationId = organizationId });

            return result.Ok;
        }

        public bool AddToSearch(string organizationId, string encryptedText, string contentId, string contentType)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SearchItemDto>> Search(string organizationId, string searchKey)
        {
            throw new System.NotImplementedException();
        }
    }
}