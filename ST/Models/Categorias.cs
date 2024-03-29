﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("Categorias")]
    public partial class Categorias
    {
        public Categorias()
        {
            Pregunta = new HashSet<Pregunta>();
        }
        public int CategoriaId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
    }
}