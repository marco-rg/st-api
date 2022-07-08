using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("LocalesNacionales")]
    public partial class LocalesNacionales
    {
        public LocalesNacionales()
        {
            Encuestas = new HashSet<Encuestas>();
        }
        public string Zona { get; set; }
        public string Ciudad { get; set; }
        public int CodigoLocal { get; set; }
        public string NombreLocal { get; set; }
        public string Provincia { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Gerente { get; set; }
        public string FormatoLocal { get; set; }
        public virtual ICollection<Encuestas> Encuestas { get; set; }
    }
}