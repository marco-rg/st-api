using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("Encuestas")]
    public class Encuestas
    {
        public Encuestas()
        {
            EncuestasDetalle = new HashSet<EncuestasDetalle>();
        }
        public long EncuestaId { get; set; }
        public int? CodigoLocal { get; set; }
        public string EncargadoId { get; set; }
        public DateTime? CreadoAl { get; set; }
        public decimal CalificacionMarca { get; set; }
        public decimal CalificacionTelefono { get; set; }
        public DateTime? ModificadoAl { get; set; }
        public string Observacion { get; set; }
        public string UserCreatorId { get; set; }
        public string UserModifierId { get; set; }
        public string UserDeleterId { get; set; }
        public DateTime? EliminadoAl { get; set; }
        public bool? EstaEliminado { get; set; }

        public virtual LocalesNacionales LocalesNacionales { get; set; }
        
        public virtual ICollection<EncuestasDetalle> EncuestasDetalle { get; set; }
    }
}