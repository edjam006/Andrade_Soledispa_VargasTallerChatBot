using System;
using Andrade_Soledispa_VargasTallerChatBot.Interfaces;
using Andrade_Soledispa_VargasTallerChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace Andrade_Soledispa_VargasTallerChatBot.Repositories
{
    // Implementación que guarda la respuesta de IA en la base de datos
    public class RespuestaRepository : IRespuestaRepository
    {
        private readonly AppDbContext _context;

        // Inyectamos el contexto de base de datos
        public RespuestaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método para guardar una respuesta
        public async Task GuardarRespuestaAsync(RespuestaIA respuesta)
        {
            _context.Respuestas.Add(respuesta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RespuestaIA>> ObtenerHistorialAsync()
        {
            return await _context.Respuestas
                                 .OrderByDescending(r => r.Fecha)
                                 .ToListAsync();
        }


    }
}
