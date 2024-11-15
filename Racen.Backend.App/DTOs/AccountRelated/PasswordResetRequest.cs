using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Racen.Backend.App.DTOs.AccountRelated
{
    public class PasswordResetRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}