using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace Entities.POCO
{
    public class ApiResponseAuth(string username, string name, string host, string ipv4, string location, string message, DateTime tokenExpirationTime, string securityToken)
    {
        /// <summary>Returns the usercode or e-mail who made the request.</summary>
        /// <example>john.doe@mycompany.org</example>
        [Required]
        [Description("Returns the usercode or e-mail who made the request.")]
        public string? Username { get; set; } = username;

        /// <summary>Returns the username who made the request.</summary>
        /// <example>John Doe</example>
        [Description("Returns the username who made the request.")]
        public string? Name { get; set; } = name;

        /// <summary>Returns the API Host.</summary>
        /// <example>https://api.codis365.cat</example>
        [Description("Returns the API Host")]
        public string? Host { get; set; } = host;

        /// <summary>Returns the username who made the request.</summary>
        /// <example>89.56.75.140</example>
        [Description("Returns the username IPv4")]
        public string? IPv4 { get; set; } = ipv4;

        /// <summary>Returns the username location based in IPv4.</summary>
        /// <example>Barcelona</example>
        [Description("Returns the username location based in IPv4")]
        public string? Location { get; set; } = location;

        /// <summary>
        /// Gets or sets the message about the API response.
        /// <example>The JWT bearer token has been successfully retrieved and is ready for use in API requests.</example>
        /// </summary>
        [Required]
        [Display(Name = "Message", Description = "Provides a message about the API response.")]
        public string Message { get; set; } = message;

        /// <summary>Returns the expiration time of the JWT Bearer Token. </summary>
        /// <example>2020-05-31T15:34:08</example>
        public DateTime? TokenExpirationTime { get; set; } = tokenExpirationTime;

        /// <summary>Returns a serialized JWT Bearer Token, which is required for subsequent API method executions. Ensure to use this token in all future API requests to authenticate and authorize access to the API's functionality. Remember that the token will be valid for the next 15 minutes.</summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9AeyJ1bmlxdWVfbmFtZSIdIjk5OTk6OTk5OSIsIlVzZXIiOiJqb3JkaS5iYWxjZWxsc0Byb2lzdGVjaC5jb20iLCJVc2VySWQiOiI5OTk5OTk5OTkiLCJuYmYiOjE1OTAyNzEzNDgsImV4cCI6MTU5MTU4NTM0OCwiaWF0IjoxNTkwMjcxMzQ4fQ6JkWJ1obup6eHtfmTuAVIkq55hsZqTMRMKW1itpt7t4</example>
        public string? Token { get; set; } = securityToken;
    }
}
