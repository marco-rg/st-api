using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST.Models
{
    public class EncuestasDetalle
    {
        public EncuestasDetalle()
        {

        }
        public long EncuestaDetalleId { get; set; }
        public long EncuestaId { get; set; }
        public int PreguntaId { get; set; }
        public int Peso { get; set; }
        public int Puntaje { get; set; }
        public int Resultado { get; set; }
        public decimal Porcentaje { get; set; }
        public string Comentario { get; set; }
        public string Adjunto { get; set; }
    }
}