namespace SearchService.Business.Entities {
    public class SearchHistory : BaseEntity
    {
        public string OrganizationId { get; set; }
        public string SearchText { get; set; }
    }
}