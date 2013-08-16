namespace SearchService.Business.Entities {
    public class Search : BaseEntity
    {
        public string SearchKey { get; set; }
        public double Score { get; set; }

        public string EncryptedTitle { get; set; }
        public string ContentType { get; set; }
        public string ContentId { get; set; }
        public string OrganizationId { get; set; }
    }
}