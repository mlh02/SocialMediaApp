 using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia_ApplicationV1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        public string Image { get; set; }

    }
}
