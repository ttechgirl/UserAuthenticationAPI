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

        [Required(ErrorMessage = "Lastname is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        //[Required(ErrorMessage = "Username is required")]
       // public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "password must be at least 8 characters")]
        public string? Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password mismatched, try again")]
        public string? ConfirmPassword { get; set; }

    }
}
