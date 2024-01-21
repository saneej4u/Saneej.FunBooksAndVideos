using Microsoft.EntityFrameworkCore;
using Saneej.FunBooksAndVideos.Data.Context;
using Saneej.FunBooksAndVideos.Order.Service.Models;
using Microsoft.Extensions.DependencyInjection;
using Saneej.FunBooksAndVideos.Order.Service;
using Saneej.FunBooksAndVideos.Order.Repository;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Order.Api.Modules
{
    public static class FunBooksAndVideosModule
    {
        public static IServiceCollection AddFunBooksAndVideosModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure  appconfig values
            services.Configure<FunBooksAndVideosOptions>(configuration.GetSection("funBooksAndVideos"));
            
            services.Scan(sc => sc.FromCallingAssembly()
            .FromAssemblies(typeof(IFunBooksAndVideosServiceMarker).Assembly)
            .AddClasses()
            .AsImplementedInterfaces()
            .WithTransientLifetime());

            services.Scan(sc => sc.FromCallingAssembly()
            .FromAssemblies(typeof(IFunBooksAndVideosRepositoryMarker).Assembly)
            .AddClasses()
            .AsImplementedInterfaces()
            .WithTransientLifetime());

            services.AddDbContext<FunBooksAndVideosContext>();
            
            
            // TODO: Replace InMemory with SQL
            //services.AddDbContext<FunBooksAndVideosContext>(options => options.UseSqlServer(configuration.GetConnectionString("FunBooksAndVideosContext")));

            //string connectionString = configuration.GetConnectionString("FunBooksAndVideosContext");
            //if (string.IsNullOrWhiteSpace(connectionString))
            //{
            //    throw new Exception("Check configuration build. Unable to resolve connection string.");
            //}

            return services;
        }

        private static bool ExcludeNonTransientTypes(Type type)
        {
            return
                !type.Name.EndsWith($"{nameof(ProductViewModel)}", StringComparison.Ordinal);
        }
    }

}
