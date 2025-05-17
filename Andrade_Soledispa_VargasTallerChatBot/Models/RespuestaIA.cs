using System;
using System.ComponentModel.DataAnnotations;

namespace Andrade_Soledispa_VargasTallerChatBot.Models
{
    // Tuvimos que crear un modelo por los atributos que debe tener la respuesta de cada interaccion con los chatbots
    public class RespuestaIA
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Respuesta { get; set; } 

        [Required]
        public DateTime Fecha { get; set; } 

        [Required]
        public string Proveedor { get; set; } 

        [Required]
        public string GuardadoPor { get; set; } 
    }
}
