using TicketSystem.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.API.Configurations
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            string connString = configuration.GetConnectionString("TicketSystemConnection");

            services.AddDbContext<TicketSystemDbContext>(options =>
            {
                options.UseSqlServer(connString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    //sqlOptions.EnableRetryOnFailure();
                });
            });
        }
    }
}
