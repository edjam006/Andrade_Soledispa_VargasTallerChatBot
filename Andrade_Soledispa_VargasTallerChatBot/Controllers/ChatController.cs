using Microsoft.AspNetCore.Mvc;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;

namespace Andrade_Soledispa_VargasTallerChatBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        // Inyectamos el servicio que se conecta con Gemini y ChatGPT
        public ChatController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("preguntar")]
        public async Task<IActionResult> Preguntar([FromBody] PreguntaRequest request)
        {
            
            if (string.IsNullOrWhiteSpace(request.Pregunta))
                return BadRequest("La pregunta no puede estar vacía.");

            string respuesta;

            // Le enviamos la pregunta al proveedor necesario mediante condicionales 
            if (request.Proveedor.ToLower() == "chatgpt")
            {
                respuesta = await _chatBotService.ObtenerRespuestaDeChatGPT(request.Pregunta);
            }
            else if (request.Proveedor.ToLower() == "gemini")
            {
                respuesta = await _chatBotService.ObtenerRespuestaDeGemini(request.Pregunta);
            }
            else
            {
                return BadRequest("Proveedor no válido. Usa 'chatgpt' o 'gemini'.");
            }

            // Devolvemos la respuesta como un objeto JSON
            return Ok(new { respuesta });
        }
    }

    
    public class PreguntaRequest
    {
        public string Pregunta { get; set; }    
        public string Proveedor { get; set; }   
    }
}
