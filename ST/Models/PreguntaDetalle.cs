using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("PreguntaDetalle")]
    public class PreguntaDetalle
    {
        public PreguntaDetalle()
        {

        }
        public int PreguntaDetalleId { get; set; }
        public int PreguntaId { get; set; }
        public string Descripcion { get; set; }
        public bool EsCorrecta { get; set; }
        public DateTime CreadoAl { get; set; }
        public DateTime? ModificadoAl { get; set; }
        public string UserCreatorId { get; set; }
        public string UserModifierId { get; set; }
        public bool EstaEliminado { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}