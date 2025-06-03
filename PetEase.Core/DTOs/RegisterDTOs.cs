using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.DTOs
{
    public class RegisterDTOs
    {
        [Required,MaxLength(450)]
        public string Name {  get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(256)]
        public string Password { get; set; }
    }
}
