using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class UserViewModel 
    {
        public Guid Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Gender { get; set; }
        public string? Occupation{ get; set; }
    }
}
