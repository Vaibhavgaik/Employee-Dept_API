using System.ComponentModel.DataAnnotations;

namespace JewelAPI.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string ClearTextPassword { get; set; } // New property for accepting password input

        // Add a property for the "Password" field
        public string Password { get; set; }
    }
}
