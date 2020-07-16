
using System.ComponentModel.DataAnnotations;
using BeerDemo.Attributes;

namespace BeerDemo.Models
{
    public class UserRating
    {
        public int BeerId { get; set; }
        [Required]
        [ValidateEmail(ErrorMessage = "Invalid Email")]
        public string UserName { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
