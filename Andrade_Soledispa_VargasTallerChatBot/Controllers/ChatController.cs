using Microsoft.AspNetCore.Mvc;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;
using Andrade_Soledispa_VargasTallerChatBot.Repositories;
using Andrade_Soledispa_VargasTallerChatBot.Models;

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
            if (request.Proveedor.ToLower() == "groq")
            {
                respuesta = await _chatBotService.ObtenerRespuestaDeGroq(request.Pregunta);
            }
            else if (request.Proveedor.ToLower() == "gemini")
            {
                respuesta = await _chatBotService.ObtenerRespuestaDeGemini(request.Pregunta);
            }
            else
            {
                return BadRequest("Proveedor no válido. Usa 'chatgpt' o 'gemini'.");
            }

            var registro = new RespuestaIA
            {
                Respuesta = respuesta,
                Fecha = DateTime.Now,
                Proveedor = request.Proveedor,
                GuardadoPor = "Eduardo" 
            };

            await _respuestaRepository.GuardarRespuestaAsync(registro);

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
