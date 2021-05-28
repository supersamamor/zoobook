using System.ComponentModel.DataAnnotations;
using System;

namespace ZEMS.Data.Models
{
    public class Employee : BaseEntity
    {          
		[StringLength(255)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(255)]
        [Required]
        public string MiddleName { get; set; }
        [StringLength(255)]
        [Required]
        public string LastName { get; set; }
        
    }
}
