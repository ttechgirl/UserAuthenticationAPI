using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.RegisterViewModel
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "password must be at least 8 characters")]
        public string? Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password mismatched!")]
        public string? ConfirmPassword { get; set; }
        public string? token { get; set; }
    }
}
