using Microsoft.EntityFrameworkCore;
using Andrade_Soledispa_VargasTallerChatBot.Models;
using System.Collections.Generic;

namespace Andrade_Soledispa_VargasTallerChatBot
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RespuestaIA> Respuestas { get; set; }
    }
}
