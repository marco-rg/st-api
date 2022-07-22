using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    public class Imagenes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ImagenId { get; set; }

        public long EncuestaId { get; set; }

        [Column(TypeName = "image")]
        public byte[] Imagen { get; set; }

        [Column(TypeName = "text")]
        public string RutaImagen { get; set; }

        [Column(TypeName = "text")]
        public string NombreImagen { get; set; }

        //[Column(TypeName = "text")]
        public string ImagenBase64String { get; set; }

        [Column(TypeName = "text")]
        public string UserCreatorId { get; set; }

        public DateTime? CreadoAl { get; set; }

        [Column(TypeName = "text")]
        public string UserModifierId { get; set; }

        public DateTime? ModificadoAl { get; set; }

        [Column(TypeName = "text")]
        public string UserDeleterId { get; set; }

        public DateTime? EliminadoAl { get; set; }

        public bool? EstaEliminado { get; set; }

        public string Comentario { get; set; }

        public virtual Encuestas Encuestas { get; set; }
    }
}