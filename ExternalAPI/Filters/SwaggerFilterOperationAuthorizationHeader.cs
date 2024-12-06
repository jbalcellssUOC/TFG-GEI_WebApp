using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExternalAPI.Filters
{
    public class SwaggerFilterOperationAuthorizationHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var noAuthRequired = context.ApiDescription.CustomAttributes().Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

            if (!noAuthRequired)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
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
                            new List<string>()
                        }
                    }
                };
            }
        }
    }
}
