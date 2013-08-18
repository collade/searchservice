namespace SearchService.Test
{
    using System;

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

        protected ISearchService _service;

        protected const string organizationId = "51fb4622902c7f0fecca0343";
        protected const string organizationKey = "xyz";

        private void ClearCollections()
        {
            this._organizationRepo.Clear();
        }

        [SetUp]
        public void Setup()
        {
            _cryptoHelper = new CryptoHelper();
            _organizationRepo = new EntityRepository<Organization>();

            this.ClearCollections();

            _service = new SearchService(_organizationRepo);
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

            Assert.AreEqual(true, _service.AddToSearch(organizationId, encText, "1", "test"));
        }
    }
}
