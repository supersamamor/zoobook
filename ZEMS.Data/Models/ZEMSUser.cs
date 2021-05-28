using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZEMS.Data.Models
{
    public class ZEMSUser 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        [Required]
        public string FullName { get; set; }
        public IdentityUser Identity { get; set; }
    }
}
