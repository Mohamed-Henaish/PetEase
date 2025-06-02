using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using PetEase.Core.DTOs;

namespace PetEase.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public BotController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = config["VetAI:ApiKey"];
        }
        [HttpPost("ask")]
        public async Task<IActionResult> AskVetBot([FromBody] AskVetBotDTOs requestModel)
        {
            string responseContent = await GenerateVetBotResponseAsync(requestModel);
            return Ok(responseContent);
        }

        // Helper
        private async Task<string> GenerateVetBotResponseAsync(AskVetBotDTOs request)
        {
            var systemPrompt = @$"
                You are VetAI, an expert veterinary assistant with 20 years of experience. 
                Provide detailed, accurate advice for animal health concerns. Consider:
                - Animal type/species
                - Symptoms and duration
                - Age and weight if provided
                - Potential emergencies
                - Home care vs vet-needed situations

                Respond in language: {request.Lang}

                Structure your response with:
                1. Likely Diagnosis (most probable causes)
                2. Immediate Actions (what to do now)
                3. Home Treatment (if safe to treat at home)
                4. Red Flags (when to see vet immediately)
                5. Prevention Tips";

            var fullPrompt = $"{systemPrompt}\n\nQuestion: {request.Prompt}\n".Trim();

            var requestBody = new
            {
                model = "gemini-2.0-flash",
                contents = new[]
                {
                    new{
                        role = "user",
                        parts = new[]
                        {
                            new { text = fullPrompt }
                        }
                    }
                }
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}"
            );

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
