using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ST.Models
{
    [Table("MetaDetalle")]
    public class MetaDetalle
    {
        [Key]
        public int MetaDetalleId { get; set; }
        public int MetaId { get; set; }
        public byte Mes { get; set; }
        public decimal? MetaCe { get; set; }
        public decimal? CumplimientoCe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeCe { get; set; }

        public decimal? MetaCh { get; set; }
        public decimal? CumplimientoCh { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeCh { get; set; }

        public decimal? MetaSe { get; set; }
        public decimal? CumplimientoSe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeSe { get; set; }

        public virtual Meta Meta { get; set; }
    }
}