using Andrade_Soledispa_VargasTallerChatBot.Models;

namespace Andrade_Soledispa_VargasTallerChatBot.Interfaces
{
    // Define los métodos que usará el repositorio de respuestas
    public interface IRespuestaRepository
    {
        Task GuardarRespuestaAsync(RespuestaIA respuesta);
        Task<List<RespuestaIA>> ObtenerHistorialAsync();

    }
}
