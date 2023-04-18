using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorFinder.Model
{
    public class UserProfile
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public AppUsers User { get; set; }
        public Guid RoleId { get; set; }
        public AppRoles Role { get; set; }
    }
}
