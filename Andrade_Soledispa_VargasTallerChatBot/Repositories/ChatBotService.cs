using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Andrade_Soledispa_VargasTallerChatBot.Repositories
{

    public class ChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _groqApiKey;
        private readonly string? _geminiApiKey;
       



        // Constructor que recibe la configuración appsettings para extraer las API Keys
        public ChatBotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _groqApiKey = configuration["Apis:groqApiKey"];
            _geminiApiKey = configuration["Apis:GeminiApiKey"];
               

        }

        public async Task<string> ObtenerRespuestaDeGroq(string pregunta)
        {
            var requestBody = new
            {
                model = "llama3-8b-8192", // modelo soportado por Groq
                messages = new[]
                {
            new { role = "user", content = pregunta }
        }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _groqApiKey);

            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error desde Groq:\n{result}";
            }

            using var doc = JsonDocument.Parse(result);

            if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0 &&
                choices[0].TryGetProperty("message", out var message) &&
                message.TryGetProperty("content", out var contentJson))
            {
                return contentJson.GetString() ?? "Respuesta vacía desde Groq.";
            }

            return "No sirvio tampoco";
        }


        public async Task<string> ObtenerRespuestaDeGemini(string pregunta)
        {
            // Estructura json que espera la API de Gemini
            var requestBody = new
            {
                contents = new[] {
            new {
                parts = new[] {
                    new { text = pregunta }
                }
            }
        }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear(); // Limpia headers
                                                       // ❌ No agregues Authorization con Bearer
                                                       // _httpClient.DefaultRequestHeaders.Authorization = ...

            var response = await _httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=" + _geminiApiKey, content);

            var result = await response.Content.ReadAsStringAsync();

            // Para debug:
            // return result;

            using var doc = JsonDocument.Parse(result);
            if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0 &&
                candidates[0].TryGetProperty("content", out var contentJson) &&
                contentJson.TryGetProperty("parts", out var parts) &&
                parts.GetArrayLength() > 0 &&
                parts[0].TryGetProperty("text", out var textJson))
            {
                return textJson.GetString() ?? "Respuesta vacía de Gemini.";
            }

            return result;

        }

    }
}
