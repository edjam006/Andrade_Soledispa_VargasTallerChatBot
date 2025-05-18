namespace Andrade_Soledispa_VargasTallerChatBot.Interfaces
{
    public interface IChatBotService
    {
        Task<string> ObtenerRespuestaDeGemini(string pregunta);
        Task<string> ObtenerRespuestaDeGroq(string pregunta);

    }
}
