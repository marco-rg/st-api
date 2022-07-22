using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("EncuestasDetalle")]
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
        public virtual Encuestas Encuestas { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}