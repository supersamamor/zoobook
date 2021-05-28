using System;

namespace ZEMS.Core.Models
{
    public class Employee : BaseModel
    {      
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        
        public void UpdateFrom(string firstName, string middleName, string lastName) 
        {            
			this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            
        }    
    }
}
