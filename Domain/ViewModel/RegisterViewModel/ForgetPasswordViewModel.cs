using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.RegisterViewModel
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

    }
}
