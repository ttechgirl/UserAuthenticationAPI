using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AppRoles:IdentityRole<Guid>
    {
       // public Guid Id { get; set; }
       //seed data with the Identityrole properties
    }
}
