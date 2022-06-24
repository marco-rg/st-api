using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("UserSystem")]
    public partial class UserSystem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSystem()
        {
            //LaboratoryDetail = new HashSet<LaboratoryDetail>();
            //PathologicalDataInput = new HashSet<PathologicalDataInput>();
            //Sampling = new HashSet<Sampling>();
            //Sampling1 = new HashSet<Sampling>();
            //Sampling2 = new HashSet<Sampling>();
            //SamplingDetail = new HashSet<SamplingDetail>();
            //Visit = new HashSet<Visit>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long usrId { get; set; }

        public long userIdAquasym { get; set; }
        public string usrLogon { get; set; }

        public string usrFirstName { get; set; }


        public string usrLastName { get; set; }


        public string usrEmail { get; set; }

        public long? usrLanguage { get; set; }

        public long? usrDefaultCompany { get; set; }

        public bool? usrExternal { get; set; }

        public bool? usrActive { get; set; }

        public bool usrBelongHA { get; set; }
        public long? usrDegree { get; set; }

        public bool usrReportNotice { get; set; }
        public long? usrPosition { get; set; }

        public DateTime? CreationTime { get; set; }

        [Column(TypeName = "text")]
        public string CreatorUserID { get; set; }

        [Column(TypeName = "text")]
        public string LastModifierID { get; set; }

        public DateTime? LastModificationTime { get; set; }

        [Column(TypeName = "text")]
        public string DeleterUserID { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool? IsDeleted { get; set; }

        [Column(TypeName = "text")]
        public string usrCompanyList { get; set; }

        [Column(TypeName = "text")]
        public string usrPermissions { get; set; }

        public string usrRefreshToken { get; set; }

        public string usrSign { get; set; }
        public string usrAppVersion { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LaboratoryDetail> LaboratoryDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PathologicalDataInput> PathologicalDataInput { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Sampling2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SamplingDetail> SamplingDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visit> Visit { get; set; }*/

        public virtual SystemParameter SystemParameter { get; set; }

        public virtual SystemParameter SystemParameter1 { get; set; }
    }
}