using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.UserDtos
{
    public class RegisterUser : IValidatableObject
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

       

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult(
                    "Password and confirmation password do not match.",
                    new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
