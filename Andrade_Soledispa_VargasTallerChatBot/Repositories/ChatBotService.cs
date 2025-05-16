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
        private readonly string _chatGptApiKey;
        private readonly string _geminiApiKey;

        // Constructor que recibe la configuración appsettings para extraer las API Keys
        public ChatBotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _chatGptApiKey = configuration["Apis:ChatGptApiKey"];
            _geminiApiKey = configuration["Apis:GeminiApiKey"];
        }

       
        public async Task<string> ObtenerRespuestaDeChatGPT(string pregunta)
        {
            // Formato JSON requerido por OpenAI, esto nos ayudo chat gpt ya que queriamos hacer un chatbot real, donde se envien los textos mediante json, y esta fue la solucion que nos dio gpt usar JSON
            // para enviar mensajes con rol "user" y contenido dinamico como una forma para integrar el modelo de OpenAI desde el backend .NET con HttpClient. Este mismo proceso se hizo con gemini por lo que no se comentaría
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
                    new { role = "user", content = pregunta }
                }
            };

            // Convertimos el objeto a JSON
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // Autorización con API Key de OpenAI
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatGptApiKey);

            // Enviamos la solicitud POST a la API de OpenAI
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var result = await response.Content.ReadAsStringAsync();

            // Procesamos el JSON de la respuesta para extraer solo el contenido
            using var doc = JsonDocument.Parse(result);
            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();
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
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _geminiApiKey);

            var response = await _httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=" + _geminiApiKey, content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            return doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString();
        }
    }
}
