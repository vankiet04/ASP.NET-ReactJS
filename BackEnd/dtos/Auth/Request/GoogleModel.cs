using System.ComponentModel.DataAnnotations;

namespace BackEnd.dtos.Auth.Request
{
    public class GoogleModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
    }
}