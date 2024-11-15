using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Racen.Backend.App.DTOs.AccountRelated
{
    public class PasswordReset
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        [MinLength(1)]
        public required string NewPassword { get; set; }
    }
}