using System;
using System.ComponentModel.DataAnnotations;

namespace ConstantPasswordGenerator.ViewModel
{
    public class GeneratePasswordViewModel
    {
        [Required]
        public string Domain { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Range { get; set; } = "5:10";

        public string Step { get; set; } = "1";
    }
}
