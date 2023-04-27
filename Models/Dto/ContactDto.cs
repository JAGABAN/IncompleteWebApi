using System.ComponentModel.DataAnnotations;

namespace MyPersonalProject.Models.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }

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

        public IFormFile File { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
