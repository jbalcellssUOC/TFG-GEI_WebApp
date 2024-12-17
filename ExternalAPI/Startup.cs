using Asp.Versioning;
using AspNetCoreRateLimit;
using AutoMapper;
using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Mappings;
using DeviceDetectorNET.Parser.Device;
using Entities.Data;
using ExternalAPI.Filters;
using ExternalAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using NLog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO.Compression;
using System.Text;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;

namespace ExternalAPI
{
    /// <summary>
    /// Startup class for the Codis365 RESTful API.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration property.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices method.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            Logger logger = LogManager.GetLogger("");                                   // Get NLog logger
            LogManager.Configuration.Variables["LoggerFileName"] = "Codis365Backend";   // Set NLog filename pre/suffix
            SqlConnectionStringBuilder SQLbuilder = new SqlConnectionStringBuilder(Configuration["Database:ConnectionString"]);
            string DB_Server = SQLbuilder.DataSource;
            string DB_Catalog = SQLbuilder.InitialCatalog;
            string DB_UserId = SQLbuilder.UserID;
            string DB_Pass = SQLbuilder.Password;

            LogManager.Configuration.Variables["DB_Server"] = DB_Server;    // Set DB Server for NLog
            LogManager.Configuration.Variables["DB_Catalog"] = DB_Catalog;  // Set Database for NLog
            LogManager.Configuration.Variables["DB_UserId"] = DB_UserId;    // Set DB UserId NLog
            LogManager.Configuration.Variables["DB_Pass"] = DB_Pass;        // Set DB Password for NLog

            LogManager.Configuration.Variables["TargetMail"] = Configuration["NLog:TargetMail"];            // Set Target Email for NLog

            LogManager.Configuration.Variables["smtpServer"] = Configuration["EmailSettings:smtpServer"];   // Set SMTP Server for NLog
            LogManager.Configuration.Variables["smtpPort"] = Configuration["EmailSettings:smtpPort"];       // Set SMTP Port for NLog
            LogManager.Configuration.Variables["smtpEmail"] = Configuration["EmailSettings:smtpEmail"];     // Set SMTP Email for NLog
            LogManager.Configuration.Variables["smtpUser"] = Configuration["EmailSettings:smtpUser"];       // Set SMTP User for NLog
            LogManager.Configuration.Variables["smtpPass"] = Configuration["EmailSettings:smtpPass"];       // Set SMTP password for NLog

            // SQL Server Database connection 
            services.AddDbContext<BBDDContext>(options => options.UseSqlServer(SQLbuilder.ConnectionString)
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors());

            BLServiceCollection.GetServiceCollection(services);     // Get service collection from IServiceCollection

            // Add compression middleware
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });

            // Add services to the container.
            services.AddCors();             // Add CORS
            services.AddMemoryCache();      // Add Memory Cache
            services.AddControllers();      // Add Controllers

            // Configure API IP Rate Limit middelware
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddSingleton<ISystemUtilsHelper, SystemUtilsHelper>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();                              // Add HttpContextAccessor

            // Configure API Versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Configure API Secret Key
            var ApiKeySecret = Encoding.ASCII.GetBytes(Configuration["APIKeySettings:Secret"]!);

            // Configure JWT Bearer Token
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(ApiKeySecret),
                        ValidateLifetime = true, // When receiving a token, check that it is still valid.
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddEndpointsApiExplorer();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
            c.EnableAnnotations();
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (apiDesc.HttpMethod == null) return false;
                return true;
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Codis365 RESTful API",
                Version = "v1.0",
                Description =
                  "<strong>We are glad to have your here! In our developer\'s hub you\'ll find everything you need to interact with our platform.</strong> <br /><br />" +
                  "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"/images/codis365-logo.jpg\" alt =\"JWTBearerImg image\"></p><br /> <br />" +
                  "The Codis365 Web API is organized around REST, using HTTP responses code to keep you informed about what\'s going on. Our endpoints will return metada in JSON format. All the \"List\" methods are GET requests and can be paginated thus you can get more pages if needed by query params i.e. <strong>?page=2</strong>.  <br /> <br />All requests are validated against an API JWT Bearer Token. You can obtain it manually from the \"authenticate\" Web API method (as you\'ll see in documented below). <img class=\"JWTBearerImg\" src=\"https://cdn2.auth0.com/docs/media/articles/api-auth/client-credentials-grant.png\" alt=\"JWT Bearer Token diagram\" width=\"600\" height =\"228\">  <br /> <br />We tried to keep the documentation as clear and simple as possible. Thus you can test our endpoints with your own API Token and see the responses code directly. Additionally, if you wish, you can use the <strong><a href=\"https://www.postman.com\" target=\"_blank\">POSTMAN</a></strong> program to test all available methods." +
                  "",

                Contact = new OpenApiContact() { Name = "UOC Treball Final de Grau,", Email = "jbalcellss@uoc.edu", Url = new Uri("https://api.codis365.cat") },
                Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        // Logo Extension
                        {"x-logo", new OpenApiObject
                            {
                                { "url", new OpenApiString("../images/codis365-logo.png")},
                                { "altText", new OpenApiString("API Logo")}
                            }
                        },
                    }
            });

            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        new string[] {}
                    }
                });

                // Add a security definition to the Swagger document
                c.AddSecurityDefinition("JWT Bearer Token", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Use a JWT Bearer Token to communicate with the Codis365 RESTful API. Notice! Always enter the token in the following format <strong>{Bearer eyJhbGciOiJIUzI1NiIs...}</strong><br /><br />" +
                    "JSON Web Token (<strong><a href=\"https://jwt.io/introduction/\" target =\"_blank\">JWT</a></strong>) is an open standard <strong><a href=\"https://tools.ietf.org/html/rfc7519\" target=\"_blank\">RFC 7519</a></strong> that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information is verified and trusted digitally signed using a secret with the HMAC algorithm.<br /> <br />" +
                    "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"https://cdn2.auth0.com/docs/media/articles/api-auth/client-credentials-grant.png\" alt=\"JWT Bearer Token diagram\" width=\"600\" height =\"228\"></p>" +
                    "The output is three Base64-URL strings separated by dots that can be easily passed in HTML and HTTP environments, while being more compact when compared to XML-based standards such as SAML. The following shows a JWT that has the previous header and payload encoded, and it is signed with a secret." +
                    "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"https://cdn.auth0.com/content/jwt/encoded-jwt3.png\" alt=\"JWT Bearer Token coded sample\" width=\"600\" height =\"138\"></p><br /> <br />"
                    ,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Add a security requirement to the Swagger document
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT Bearer Token"
                            }
                        },
                        new string[] {}
                    }
                });

                c.OperationFilter<SwaggerFilterOperationAuthorizationHeader>(); // Add a custom operation filter which sets default values for the Authorization header
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); // Resolve conflicts between actions

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Add filters to fix enums
                c.AddEnumsWithValuesFixFilters(o =>
                {
                    o.ApplySchemaFilter = true; // add schema filter to fix enums (add 'x-enumNames' for NSwag) in schema
                    o.ApplyParameterFilter = true; // add parameter filter to fix enums (add 'x-enumNames' for NSwag) in schema parameters
                    o.ApplyDocumentFilter = true; // add document filter to fix enums displaying in swagger document
                    o.IncludeDescriptions = true; // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' for schema extensions) for applied filters
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments; // get descriptions from DescriptionAttribute then from xml-comments
                    o.IncludeXmlCommentsFrom(xmlPath); // get descriptions from xml-file comments on the specified path should use "options.IncludeXmlComments(xmlFilePath);" before
                });
            });

            // Register the Swagger generator, defining 1 or more Swagger documents. explicit opt-in - needs to be placed after AddSwaggerGen()
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIpRateLimiting();

            // Enable OPENAPI middleware UI.
            app.UseSwaggerUI(c => {
                c.DocumentTitle = "Codis365 RESTful API v1"; // Html Title
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelExpandDepth(5);
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.List);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.EnablePersistAuthorization();
                c.ShowExtensions();
                c.EnableValidator();
                c.InjectStylesheet("custom-ui/custom.css"); // Inject custom CSS style
                c.SwaggerEndpoint("/apidocs/v1/Codis365api.json", "Codis365 RESTful API v1"); // For Local IIS
                c.RoutePrefix = "apidocs";
                c.SupportedSubmitMethods([SubmitMethod.Get, SubmitMethod.Post]);
                c.EnableTryItOutByDefault();
            });

            // Enable REDOC middleware UI.
            app.UseReDoc(c => {
                c.DocumentTitle = "Codis365 RESTful API v1"; // Html Title
                c.RoutePrefix = "apidocs-redoc";
                c.SpecUrl("/apidocs/v1/Codis365api.json");
                c.InjectStylesheet("/apidocs-redoc/custom-ui/custom.css");
                c.EnableUntrustedSpec();
                c.ScrollYOffset(10);
                c.ExpandResponses("200");
            });

            // Swaggerbuckle. Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c => { c.RouteTemplate = "/apidocs/{documentName}/Codis365api.json"; }); // For Local IIS
            //app.MapSwagger().RequireAuthorization();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
