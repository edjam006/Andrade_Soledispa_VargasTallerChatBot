using Microsoft.AspNetCore.Mvc;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;
using Andrade_Soledispa_VargasTallerChatBot.Repositories;

namespace Andrade_Soledispa_VargasTallerChatBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly IRespuestaRepository _respuestaRepository;


        // Inyectamos el servicio que se conecta con Gemini y ChatGPT
        public ChatController(IChatBotService chatBotService, IRespuestaRepository respuestaRepository)
        {
            _chatBotService = chatBotService;
            _respuestaRepository = respuestaRepository;
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

        [HttpGet("historial")]
        public async Task<IActionResult> ObtenerHistorial()
        {
            var historial = await _respuestaRepository.ObtenerHistorialAsync();
            return Ok(historial);
        }

    }


    public class PreguntaRequest
    {
        public string Pregunta { get; set; }    
        public string Proveedor { get; set; }   
    }
}
