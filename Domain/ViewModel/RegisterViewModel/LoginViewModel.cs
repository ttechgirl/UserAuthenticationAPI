﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.RegisterViewModel
{
    public class LoginViewModel
    {
        //[Required(ErrorMessage = "Username is required")]
        //public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is  required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
