using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("Preguntas")]
    public partial class Pregunta
    {
        public Pregunta()
        {
            PreguntaDetalle = new HashSet<PreguntaDetalle>();
        }
        public int PreguntaId { get; set; }
        public string Descripcion { get; set; }
        public int TipoPreguntaId { get; set; }
        public int Peso { get; set; }
        public int? CategoriaId { get; set; }
        public DateTime CreadoAl { get; set; }
        public DateTime? ModificadoAl { get; set; }
        public string UserCreatorId { get; set; }
        public string UserModifierId { get; set; }
        public bool? EstaEliminado { get; set; }

        public virtual Categorias Categorias { get; set; }
        public virtual ICollection<PreguntaDetalle> PreguntaDetalle { get; set; }
    }
}