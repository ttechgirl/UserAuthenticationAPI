using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AppUsers :IdentityUser<Guid> 
    {
        //public Guid Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public int Gender { get; set; }
        public string? Occupation { get; set; }
        public DateTime? LastLoginDate { get; set; } = DateTime.Now;
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; } = DateTime.Now.Date;

    }
}
