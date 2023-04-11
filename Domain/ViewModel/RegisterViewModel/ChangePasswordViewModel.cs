
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.RegisterViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Current Password")]
        [StringLength(15, ErrorMessage = "Current password incorrect")]
        public string? CurrentPassword { get; set; }

        [Required,DataType(DataType.Password) ,Display(Name ="New Password")]
        [StringLength(15, ErrorMessage = "Current password incorrect")]
        public string? NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password mismatched!")]
        public string? ConfirmPassword { get; set; }
    }
}
