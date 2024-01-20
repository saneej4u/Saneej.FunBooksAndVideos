using Saneej.FunBooksAndVideos.Data.Entities;

namespace Saneej.FunBooksAndVideos.Data.Entities
{
    public class Membership : EntityBase
    {
        public int MembershipId { get; set; }
        public int CustomerId { get; set; }
        public string MembershipName { get; set; }
        public DateTime MemberShipStartedOn { get; set; }
        public DateTime MemberShipExpiresOn { get; set; }
    }
}
