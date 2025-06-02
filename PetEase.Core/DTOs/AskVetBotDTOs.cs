using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.DTOs
{
    public class AskVetBotDTOs
    {
        public string Prompt { get; set; }
        public string Lang { get; set; } = "ar";
    }
}
