using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExternalAPI.Filters
{
    public class SwaggerFilterOperationAddCodeSamples : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Extensions.Add(
                "x-codeSamples", new OpenApiObject
                {
                    ["lang"] = new OpenApiString("JavaScript"),
                    ["source"] = new OpenApiString("console.log('');")
                }
            );
        }
    }
}
