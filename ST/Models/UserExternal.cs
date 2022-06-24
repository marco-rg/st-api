using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    [Table("UserExternal")]
    public class UserExternal
    {
        public UserExternal()
        {

        }
        [Key]
        public long UserId { get; set; }
        public long? CompanyID { get; set; }
        public long? SubCompanyID { get; set; }
        public long ProfileID { get; set; }

        [StringLength(37)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Username { get; set; }

        [StringLength(150)]
        public string PassWord { get; set; }

        public DateTime? CreationTime { get; set; }
        public long? CreatorUserID { get; set; }

        public long? LastModifierUserID { get; set; }

        public long? LastModificationTime { get; set; }

        public long? DeleteUserID { get; set; }

        public long? DeletionTime { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string RefreshToken { get; set; }

        [StringLength(14)]
        public string Status { get; set; }


    }
}