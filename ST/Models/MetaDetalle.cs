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
        public short? MetaCe { get; set; }
        public short? CumplimientoCe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeCe { get; set; }

        public short? MetaCh { get; set; }
        public short? CumplimientoCh { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeCh { get; set; }

        public short? MetaSe { get; set; }
        public short? CumplimientoSe { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PorcentajeSe { get; set; }

        public virtual Meta Meta { get; set; }
    }
}