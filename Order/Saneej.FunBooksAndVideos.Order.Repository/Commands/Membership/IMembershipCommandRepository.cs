namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership
{
    public interface IMembershipCommandRepository
    {
        Task AddMembership(Data.Entities.Membership membership);
    }
}
