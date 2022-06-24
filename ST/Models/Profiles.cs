using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("Profiles")]
    public class Profiles
    {
        public Profiles()
        {

        }
        [Key]
        public long ProfilesID { get; set; }

        [StringLength(180)]
        public string Name { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Permissions { get; set; }

        public long? CreatorUserID { get; set; }

        public long? LastModifierUserID { get; set; }

        public long? DeleteUserID { get; set; }

        [StringLength(14)]
        public string Status { get; set; }


    }
}