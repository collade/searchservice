namespace SearchService.Business.Entities
{
    public class Organization : BaseEntity
    {
        public string OrganizationId { get; set; }
        public string EncryptionKey { get; set; }
    }
}