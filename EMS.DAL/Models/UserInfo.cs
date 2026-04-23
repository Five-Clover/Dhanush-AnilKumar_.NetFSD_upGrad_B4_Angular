using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Models
{
    public class UserInfo
    {
        [Key]
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public string Role { get; set; } // Admin / Participant

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
