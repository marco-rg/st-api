using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ST.Models
{
    [Table("Metas")]
    public class Meta
    {
        public Meta()
        {
            MetaDetalle = new HashSet<MetaDetalle>();
        }
        [Key]
        public int MetaId { get; set; }
        public int LocalId { get; set; }
        public int NumAnio { get; set; }
        public int MetaTotalCe { get; set; }
        public int CumplimientoTotalCe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalCe { get; set; }

        public int MetaTotalCh { get; set; }
        public int CumplimientoTotalCh { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalCh { get; set; }

        public int MetaTotalSe { get; set; }
        public int CumplimientoTotalSe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalSe { get; set; }
        public virtual ICollection<MetaDetalle> MetaDetalle { get; set; }
    }
}