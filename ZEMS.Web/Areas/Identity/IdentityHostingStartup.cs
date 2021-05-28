using Microsoft.AspNetCore.Hosting;
[assembly: HostingStartup(typeof(ZEMS.Web.Areas.Identity.IdentityHostingStartup))]
namespace ZEMS.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}