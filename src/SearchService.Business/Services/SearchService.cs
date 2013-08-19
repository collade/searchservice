namespace SearchService.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using global::SearchService.Business.Entities;
    using global::SearchService.Business.Helpers;
    using global::SearchService.Business.Repos;

    public class SearchService : ISearchService
    {
        private readonly IEntityRepository<Organization> organizationRepo;
        private readonly IEntityRepository<Search> searchRepo;
        private readonly IEntityRepository<SearchHistory> searchHistoryRepo;

        private CryptoHelper cryptoHelper;

        public SearchService(
            IEntityRepository<Organization> organizationRepo,
            IEntityRepository<Search> searchRepo,
            IEntityRepository<SearchHistory> searchHistoryRepo,
            CryptoHelper cryptoHelper)
        {
            this.organizationRepo = organizationRepo;
            this.searchRepo = searchRepo;
            this.searchHistoryRepo = searchHistoryRepo;
            this.cryptoHelper = cryptoHelper;
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

            var result = organizationRepo.Add(new Organization { CreatedBy = "System", UpdatedBy = "System", OrganizationId = organizationId, EncryptionKey = encryptionKey });

            return result.Ok;
        }

        public async Task<bool> AddToSearch(string organizationId, string text, string encryptedResultTitle, string contentId, string contentType, bool isEncrypted)
        {
            if (string.IsNullOrEmpty(organizationId) ||
                string.IsNullOrEmpty(text) ||
                string.IsNullOrEmpty(encryptedResultTitle) ||
                string.IsNullOrEmpty(contentId) ||
                string.IsNullOrEmpty(contentType))
            {
                return await Task.FromResult(false);
            }

            if (!HasOrganization(organizationId))
            {
                return await Task.FromResult(false);
            }

            contentType = contentType.ToLowerInvariant();

            var items = new List<Search>();
            var keys = new List<string>();

            var orgKey = organizationRepo.AsQueryable().First(x => x.OrganizationId == organizationId).EncryptionKey;
            if (isEncrypted)
            {
                text = cryptoHelper.Decrypt(text, orgKey);
            }

            var words = await GetSearchKeywords(text);
            foreach (var word in words)
            {
                var maxLenght = word.Length;
                items.Add(new Search
                {
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    ContentId = contentId,
                    ContentType = contentType,
                    SearchKey = BCrypt.Net.BCrypt.HashPassword(word),
                    Score = Math.Pow(2, maxLenght + 2),
                    OrganizationId = organizationId,
                    EncryptedTitle = encryptedResultTitle
                });
                keys.Add(word);

                var iLenght = word.Length;
                var jLenght = word.Length - 2;
                for (int i = 3; i < iLenght; i++)
                {
                    for (int j = 0; j < jLenght; j++)
                    {
                        if ((i + j) > iLenght)
                        {
                            break;
                        }

                        var key = word.Substring(j, i);
                        if (!keys.Contains(key))
                        {
                            keys.Add(key);
                            items.Add(new Search
                            {
                                CreatedBy = "System",
                                UpdatedBy = "System",
                                ContentId = contentId,
                                ContentType = contentType,
                                SearchKey = BCrypt.Net.BCrypt.HashPassword(key),
                                Score = Math.Pow(2, i) + (jLenght - j),
                                OrganizationId = organizationId,
                                EncryptedTitle = encryptedResultTitle
                            });
                        }
                    }
                }
            }

            searchRepo.AddBulk(items);

            return await Task.FromResult(true);
        }

        private static async Task<IEnumerable<string>> GetSearchKeywords(string text)
        {
            var rgx = new Regex(@"[^\w üğişçöÖÇŞİĞÜ]");
            text = rgx.Replace(text, string.Empty);

            var result = new List<string>();
            result.AddRange(text.ToLowerTR().Split(' ').Where(x => x.Length > 2).ToList());
            result.AddRange(text.ToUrlSlug().Replace("-", " ").Split(' ').Where(x => x.Length > 2).ToList());

            result = result.Distinct().ToList();

            return await Task.FromResult(result);
        }


        public Task<List<SearchItemDto>> Search(string organizationId, string searchKey)
        {
            var result = new List<SearchItemDto>();

            if (string.IsNullOrEmpty(organizationId) ||
                string.IsNullOrEmpty(searchKey))
            {
                return Task.FromResult(result);
            }

            if (HasOrganization(organizationId))
            {
                return Task.FromResult(result);
            }

            var items = searchRepo.AsQueryable().Where(x => x.OrganizationId == organizationId && BCrypt.Net.BCrypt.Verify(searchKey, x.SearchKey)).OrderByDescending(x => x.Score);
            
            foreach (var item in items)
            {
                result.Add(new SearchItemDto
                {
                    Title = item.EncryptedTitle
                });
            }




            return Task.FromResult(result);
        }
    }
}