using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.RegisterViewModel
{
    public class SignUpViewModel
    {
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Last name is required"), Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "First name is required"), Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [ Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        //[Required(ErrorMessage = "Username is required")]
       // public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required, DataType(DataType.PhoneNumber), Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password must be at least 8 characters")]
        public string? Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password mismatched, try again")]
        public string? ConfirmPassword { get; set; }

    }
}
