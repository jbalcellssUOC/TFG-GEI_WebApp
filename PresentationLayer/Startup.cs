namespace PresentationLayer
{
    using AutoMapper;
    using BusinessLogicLayer.Classes;
    using BusinessLogicLayer.Interfaces;
    using BusinessLogicLayer.Mappings;
    using BusinessLogicLayer.Services;
    using DataAccessLayer.Classes;
    using Entities.Data;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Net.Http.Headers;
    using NLog;
    using SecurityHubs.Hubs;
    using System;
    using System.Globalization;
    using System.IO.Compression;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.AddHttpClient();                                                   // HttpClient
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();         // HttpContextAccessor
            services.AddScoped<IUserDDService, UserDDService>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddLocalization();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.LoginPath = new PathString("/SignIn/Login");
                    options.LogoutPath = new PathString("/SignIn/Login");
                    options.AccessDeniedPath = new PathString("/SignIn/Login");
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = new PathString("/SignIn/Login");
                options.LogoutPath = new PathString("/SignIn/Login");
                options.AccessDeniedPath = new PathString("/SignIn/Login");
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddDbContext<BBDDContext>(options => options.UseSqlServer(Configuration["Database:ConnectionString"])
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors());

            services.AddBLInjectionExtensions();
            services.AddDALInjectionExtensions();

            services.AddControllersWithViews();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true);
                });
            });

            services.AddSignalR(config =>
            {
                config.EnableDetailedErrors = true;
                config.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                config.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });

            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddLocalization();
            services.AddRazorPages().AddViewLocalization();
            services.AddMvc();
        }

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es"),
                new CultureInfo("ca"),
                new CultureInfo("pt")
            };

            var locOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            // Proveedor personalizado que respeta la cookie
            locOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            {
                // Verificar la cookie primero
                var cookieProvider = new CookieRequestCultureProvider();
                var cultureResult = await cookieProvider.DetermineProviderCultureResult(context);

                // Si la cookie no existe, devuelve la cultura predeterminada
                return cultureResult ?? new ProviderCultureResult("en");
            }));

            app.UseRequestLocalization(locOptions);

            var assembly = typeof(Resources.BasicResources).Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            Console.WriteLine("Embedded Resources in Assembly:");
            foreach (var resourceName in resourceNames)
            {
                Console.WriteLine(resourceName);
            }

            Logger logger = LogManager.GetLogger("");                                   // Get NLog logger
            LogManager.Configuration.Variables["LoggerFileName"] = "Codis365Backend";   // Set NLog filename pre/suffix
            var sqlBuilder = new SqlConnectionStringBuilder(Configuration["Database:ConnectionString"]);
            LogManager.Configuration.Variables["DB_Server"] = sqlBuilder.DataSource;
            LogManager.Configuration.Variables["DB_Catalog"] = sqlBuilder.InitialCatalog;
            LogManager.Configuration.Variables["DB_UserId"] = sqlBuilder.UserID;
            LogManager.Configuration.Variables["DB_Pass"] = sqlBuilder.Password;
            LogManager.Configuration.Variables["TargetMail"] = Configuration["NLog:TargetMail"];
            LogManager.Configuration.Variables["smtpServer"] = Configuration["EmailSettings:smtpServer"];
            LogManager.Configuration.Variables["smtpPort"] = Configuration["EmailSettings:smtpPort"];
            LogManager.Configuration.Variables["smtpEmail"] = Configuration["EmailSettings:smtpEmail"];
            LogManager.Configuration.Variables["smtpUser"] = Configuration["EmailSettings:smtpUser"];
            LogManager.Configuration.Variables["smtpPass"] = Configuration["EmailSettings:smtpPass"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseResponseCompression();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null,
                KnownProxies = { IPAddress.Parse("127.0.0.1"), IPAddress.Parse("79.143.89.216") },
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseCookiePolicy();
            app.UseSession();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SecurityHub>("/securityhub", options =>
                {
                    options.Transports = HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
                    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(180);
                    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(180);
                });
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                CultureInfo.CurrentCulture = new CultureInfo("en");
                CultureInfo.CurrentUICulture = new CultureInfo("en");

                context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                await next();
            });

            // Device Detector Start
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var servicesDD = serviceScope.ServiceProvider;
                var deviceDetectorService = servicesDD.GetRequiredService<IUserDDService>();
            }
        }
    }
}
