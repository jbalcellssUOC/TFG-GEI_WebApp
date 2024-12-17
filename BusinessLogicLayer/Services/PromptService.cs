using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// PromptService class
    /// </summary>
    /// <param name="Configuration"></param>
    public class PromptService(IConfiguration Configuration) : IPromptService
    {
        /// <summary>
        /// TriggerPromptOpenAI method
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task<string> TriggerPromptOpenAI(string question)
        {
            // Get OpenAI API key and base URL from appsettings.json
            var apiKey = Configuration["OpenAISettings:APIKey"];
            //var apiKey = Configuration["OpenAISettings:DummyKey"];
            var baseUrl = Configuration["OpenAISettings:BaseUrl"];
            string? responseText;
            try
            {
                // Create a new HttpClient instance
                HttpClient client = new()
                {
                    // Set the base address & default request header for the API
                    BaseAddress = new Uri(baseUrl!),
                    DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiKey) }
                };

                string AssistantId = Configuration["OpenAISettings:AssistantKey"]!;      // Exclusive OpenAi Assistant with expecific context trained for Codis365 and Barcodes     
                string ThreadId = Configuration["OpenAISettings:AssistantThread"]!;      // Exclusive OpenAi Assistant Thread to manage messages     

                // Create a new OpenAIRequestDto instance
                var request = new OpenAIRequestDto
                {
                    Model = "gpt-3.5-turbo",                                            // Standard model gpt-3.5-turbo
                    Messages = new List<OpenAIMessageRequestDto>{
                    new() {
                        Role = "user",
                        Content = question
                    }
                },
                    MaxTokens = 100
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUrl, content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                    throw new Exception(errorResponse!.Error!.Message);
                }
                // Deserialize the response JSON to OpenAIResponseDto
                var data = JsonSerializer.Deserialize<OpenAIResponseDto>(resjson);
                responseText = data!.choices![0]!.message!.content;
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Error in TriggerPromptOpenAI");
                responseText = "Sorry, I am not able to answer that question right now.";
            }

            return responseText!;
        }
    }
}
