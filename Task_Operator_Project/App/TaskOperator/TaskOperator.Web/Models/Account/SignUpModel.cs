using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskOperator.Web.Models.Account
{
    public class SignUpModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DisplayName("First name")]
        public string FirstName { get; set; }

        [Required, DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public bool IsManager { get; set; }
    }
}