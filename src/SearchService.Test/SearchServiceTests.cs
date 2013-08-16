namespace SearchService.Test
{
    using NUnit.Framework;

    using SearchService.Business.Entities;
    using SearchService.Business.Repos;
    using SearchService.Business.Services;

    [TestFixture]
    public class SearchServiceTests
    {
        protected IEntityRepository<Organization> _organizationRepo;

        protected ISearchService _searchService;

        protected const string organizationId = "51fb4622902c7f0fecca0343";



    }
}
