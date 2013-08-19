namespace SearchService.Test
{
    using System;
    using System.Security.Cryptography;

    using NUnit.Framework;

    using SearchService.Business.Entities;
    using SearchService.Business.Helpers;
    using SearchService.Business.Repos;
    using SearchService.Business.Services;

    [TestFixture]
    public class SearchServiceTests
    {
        protected CryptoHelper _cryptoHelper;
        protected IEntityRepository<Organization> _organizationRepo;
        protected IEntityRepository<Search> _searchRepo;
        protected IEntityRepository<SearchHistory> _searchHistoryRepo;

        protected ISearchService _service;

        protected const string organizationId = "51fb4622902c7f0fecca0343";
        protected string organizationKey;

        private void ClearCollections()
        {
            this._organizationRepo.Clear();
            this._searchRepo.Clear();
            this._searchHistoryRepo.Clear();
        }

        [SetUp]
        public void Setup()
        {
            _cryptoHelper = new CryptoHelper();
            _organizationRepo = new EntityRepository<Organization>();
            _searchRepo = new EntityRepository<Search>();
            _searchHistoryRepo = new EntityRepository<SearchHistory>();

            this.ClearCollections();

            organizationKey = Convert.ToBase64String(TripleDES.Create().Key);

            _service = new SearchService(_organizationRepo, _searchRepo, _searchHistoryRepo, _cryptoHelper);
        }

        [Test]
        public void Should_save_organization_when_AddOrganization_method_called()
        {
            Assert.AreEqual(true, _service.AddOrganization(organizationId, organizationKey));
        }

        [Test]
        public void Should_check_if_organization_exists_when_HasOrganization_method_called()
        {
            _service.AddOrganization(organizationId, organizationKey);
            Assert.AreEqual(true, _service.HasOrganization(organizationId));
            Assert.AreEqual(false, _service.HasOrganization(Guid.NewGuid().ToString()));
        }

        [Test]
        public void Should_save_searchIndexItem_when_AddToSearch_method_called()
        {
            this.ClearCollections();
            _service.AddOrganization(organizationId, organizationKey);

            var aText = "just a text";
            var encText = _cryptoHelper.Encrypt(aText, organizationKey);

            Assert.AreEqual(true, _service.AddToSearch(organizationId, encText, encText, "1", "test", true).Result);
        }

        [Test]
        public void Should_find_searchIndexItem_when_Search_method_called()
        {
            this.ClearCollections();
            _service.AddOrganization(organizationId, organizationKey);

            var aText = "just a text";
            var encText = _cryptoHelper.Encrypt(aText, organizationKey);

            Assert.Greater(0, _service.Search(organizationId, "text").Result.Count);
            Assert.AreEqual(0, _service.Search(organizationId, "test").Result.Count);
        }
    }
}
