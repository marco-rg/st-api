using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("SystemParameter")]
    public partial class SystemParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemParameter()
        {
            /*LaboratoryDataInput = new HashSet<LaboratoryDataInput>();
            Parameter = new HashSet<Parameter>();
            PathologicalKpi = new HashSet<PathologicalKpi>();
            ProductionGroup = new HashSet<ProductionGroup>();
            ProductionGroup1 = new HashSet<ProductionGroup>();
            ProductionGroup2 = new HashSet<ProductionGroup>();
            ProductionGroup3 = new HashSet<ProductionGroup>();
            ProductionGroup4 = new HashSet<ProductionGroup>();
            ProductionGroup5 = new HashSet<ProductionGroup>();
            ProductionGroup6 = new HashSet<ProductionGroup>();
            ProductionGroup7 = new HashSet<ProductionGroup>();
            ProductionGroup8 = new HashSet<ProductionGroup>();
            ProductionGroup9 = new HashSet<ProductionGroup>();
            ProductionGroup10 = new HashSet<ProductionGroup>();
            ProductionGroup11 = new HashSet<ProductionGroup>();
            Sampling = new HashSet<Sampling>();
            Site = new HashSet<Site>();
            Site1 = new HashSet<Site>();
            Unit = new HashSet<Unit>();*/
            UserSystem = new HashSet<UserSystem>();
            UserSystem1 = new HashSet<UserSystem>();
        }

        [Column(TypeName = "text")]
        public string field { get; set; }

        [Column(TypeName = "text")]
        public string value { get; set; }

        [StringLength(2)]
        public string Language { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        /*public virtual ICollection<Parameter> Parameter { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LaboratoryDataInput> LaboratoryDataInput { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PathologicalKpi> PathologicalKpi { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Site> Site { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Site> Site1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup4 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup5 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup6 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup7 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup8 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup9 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup10 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionGroup> ProductionGroup11 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling { get; set; }*/

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSystem> UserSystem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSystem> UserSystem1 { get; set; }
    }
}