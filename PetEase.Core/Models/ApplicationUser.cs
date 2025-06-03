using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(450)]
        public string? Name { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
