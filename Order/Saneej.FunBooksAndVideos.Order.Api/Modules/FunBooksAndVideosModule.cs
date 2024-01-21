using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Membership;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.PurchaseOrder;
using Saneej.FunBooksAndVideos.Order.Repository.Commands.Shipping;
using Saneej.FunBooksAndVideos.Order.Repository.Queries.PurchaseOrder;
using Saneej.FunBooksAndVideos.Order.Repository.UnitOfWork;
using Saneej.FunBooksAndVideos.Order.Service.Models;
using Saneej.FunBooksAndVideos.Service.Customer;
using Saneej.FunBooksAndVideos.Service.Mappers;
using Saneej.FunBooksAndVideos.Service.PurchaseOrder;
using Saneej.FunBooksAndVideos.Service.Services.Integration;
using Saneej.FunBooksAndVideos.Service.Shipping;

namespace Saneej.FunBooksAndVideos.Order.Api.Modules
{
    public static class FunBooksAndVideosModule
    {
        public static IServiceCollection AddFunBooksAndVideosModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FunBooksAndVideosOptions>(configuration.GetSection("funBooksAndVideos"));

            //Register service
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IIntegrationHttpService, IntegrationHttpService>();
            services.AddScoped<IPurchaseOrderMapper, PurchaseOrderMapper>();

            // Register repo
            services.AddScoped<IMembershipCommandRepository, MembershipCommandRepository>();
            services.AddScoped<IPurchaseOrderCommandRepository, PurchaseOrderCommandRepository>();
            services.AddScoped<IShippingCommandRepository, ShippingCommandRepository>();
            services.AddScoped<IPurchaseOrderQueryRepository, PurchaseOrderQueryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            string connectionString = configuration.GetConnectionString("FunBooksAndVideosContext");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Check configuration build. Unable to resolve connection string.");
            }

            return services;
        }
    }
}
