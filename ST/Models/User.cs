using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("UserSystem")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            //PathologicalDataInput = new HashSet<PathologicalDataInput>();
            //Sampling = new HashSet<Sampling>();
            //Sampling1 = new HashSet<Sampling>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int userId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string userKey { get; set; }

        [Column(TypeName = "text")]
        public string userName { get; set; }

        [Column(TypeName = "text")]
        public string userDegree { get; set; }

        [Column(TypeName = "text")]
        public string userPosition { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PathologicalDataInput> PathologicalDataInput { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling1 { get; set; }*/
    }
}