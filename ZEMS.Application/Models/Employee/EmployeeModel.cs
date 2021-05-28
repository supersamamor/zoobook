using System.ComponentModel.DataAnnotations;
using System;

namespace ZEMS.Application.Models.Employee
{
    public class EmployeeModel : BaseModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PromptMessageFieldIsRequired")]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PromptMessageFieldIsRequired")]
        public string MiddleName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PromptMessageFieldIsRequired")]
        public string LastName { get; set; }
        
    }
}
