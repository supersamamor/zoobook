using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ZEMS.Web.Services.Email;
using ZEMS.Data;
using MediatR;
using ZEMS.Data.Repositories;
using AutoMapper;
using ZEMS.Logger.Extensions.DependencyInjection;
using ZEMS.Logger.Extensions.AspNetCore;
using ZEMS.Application.Models.User;
using ZEMS.Application.Models.Role;
using ZEMS.Application.ApplicationServices.Role;
using ZEMS.Application.ApplicationServices.User;
using ZEMS.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using ZEMS.Logger.Filters;
using ZEMS.Application.ApplicationServices.Employee;
using ZEMS.Application.Models.Employee;


namespace ZEMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {        
            services.AddLogCorrelation();
            services.AddOptions();
            services.Configure<ZEMSWebConfig>(Configuration.GetSection("ZEMSWebConfig"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSession();
            services.AddDbContext<ZEMSContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ZEMSContext>();

            services.AddSingleton(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Data.Models.Employee, EmployeeModel>().ReverseMap();
                    cfg.CreateMap<Data.Models.Employee, Core.Models.Employee>().ReverseMap();
                    cfg.CreateMap<Core.Models.Employee, EmployeeModel>().ReverseMap();
                    
                    cfg.CreateMap<Data.Models.ZEMSUser, UserModel>().ReverseMap();
                    cfg.CreateMap<Data.Models.ZEMSUser, Core.Models.ZEMSUser>().ReverseMap();
                    cfg.CreateMap<Core.Models.ZEMSUser, UserModel>().ReverseMap();
                    cfg.CreateMap<IdentityUser, Core.Models.IdentityUser>().ReverseMap();
                    cfg.CreateMap<IdentityRole, RoleModel>().ReverseMap();
                }
            ));

            #region Application Services
            services.AddTransient<EmployeeService>();
        
            services.AddTransient<UserService>();
            services.AddTransient<RoleService>();
            #endregion
            
            #region External Logins
            services.AddAuthentication()       
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["AuthenticationConfig:GoogleClientId"];
                googleOptions.ClientSecret = Configuration["AuthenticationConfig:GoogleClientSecret"];
            });
            #endregion

            services.AddTransient<IEmailSender, SMTPEmailService>();
            services.AddRazorPages();
            services.AddHealthChecks().AddDbContextCheck<ZEMSContext>();
            services.AddApplicationInsightsTelemetry();
            services.AddMediatR(typeof(Resource).Assembly);
            services.AddTransient<EmployeeRepository>();
            
            services.AddTransient<UserRepository>();
            services.AddHealthChecks().AddSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region Api
            services.AddHttpContextAccessor();

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add<SerilogLoggingActionFilter>();
            });
        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ZEMS API",
                    Description = "Web APIs for accessing ZEMS resources",
                });
            });
            #endregion
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            #region Api
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZEMS API");
            });
            app.UseLogCorrelation();
            #endregion
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseLogCorrelation();   
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
