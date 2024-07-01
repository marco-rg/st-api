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
        public decimal MetaTotalCe { get; set; }
        public decimal CumplimientoTotalCe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalCe { get; set; }

        public decimal MetaTotalCh { get; set; }
        public decimal CumplimientoTotalCh { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalCh { get; set; }

        public decimal MetaTotalSe { get; set; }
        public decimal CumplimientoTotalSe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalSe { get; set; }
        
        public decimal MetaTotalMi { get; set; }
        public decimal CumplimientoTotalMi { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal PorcentajeTotalMi { get; set; }
        public virtual ICollection<MetaDetalle> MetaDetalle { get; set; }
    }
}