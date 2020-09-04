using JuniorTennis.Domain.Externals;
using JuniorTennis.Infrastructure;
using JuniorTennis.Infrastructure.DataBase;
using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Infrastructure.Mail;
using JuniorTennis.Infrastructure.Storage;
using JuniorTennis.Mvc.Configurations;
using JuniorTennis.Mvc.DummyData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JuniorTennis.Mvc
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services
                .AddControllersWithViews(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(options =>
                {
                    // {0} - Action Name
                    // {1} - Controller Name
                    // {2} - Area Name
                    // {3} - Feature Name
                    // Replace normal view location entirely
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Features/{2}/{3}/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Features/{2}/{3}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                });
            services.AddDbContext<JuniorTennisDbContext>(options =>
                options.UseNpgsql(this.Configuration.GetConnectionString("JuniorTennisDbContext")));
            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<JuniorTennisDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            services.Configure<UrlSettings>(this.Configuration.GetSection("UrlSettings"));

            if (this.environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
                // memo: クライアント検証を無効にしたい場合は次のオプションをコメントアウトアウトだ
                //mvcBuilder.AddViewOptions(options =>
                //    options.HtmlHelperOptions.ClientValidationEnabled = false);
                services.AddScoped<IFileAccessor, StubFileAccessor>();
                services.AddTransient<IMailSender, StubMailSender>();
            }
            else
            {
                // todo 設定情報管理未検討
                services.AddTransient<IMailSender, SESMailSender>();
                services.AddTransient<IFileAccessor, S3FileAccessor>();
                services.Configure<SESMailSenderOptions>(this.Configuration);
                services.Configure<S3FileAccessorOptions>(this.Configuration);
            }

            Setup.ConfigureServices(services);
            ApplicationModule.ConfigureServices(services);
            ValidationAttributeModule.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetRequiredService<JuniorTennisDbContext>();
                // 未実行のMigrationを実行
                context.Database.Migrate();

                // ダミー大会データ作成
                TournamentsDummy.Create(context);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "association",
                    areaName: "Association",
                    pattern: "Association/{controller}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "identity",
                    areaName: "Identity",
                    pattern: "Identity/{controller}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
