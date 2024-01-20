namespace Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping
{
    public interface IShippingCommandRepository
    {
        Task AddShippings(List<Data.Entities.Shipping> shippings);
    }
}
