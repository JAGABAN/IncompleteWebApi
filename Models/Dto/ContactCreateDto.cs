using System.ComponentModel.DataAnnotations;

namespace MyPersonalProject.Models.Dto
{
    public class ContactCreateDto
    {
        

        [Required]
        [MaxLength(30)]
        
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }= DateTime.Now;
        
    }
}
