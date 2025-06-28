using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.Enums;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Validations
{
    public class ActorValidation : IActorValidation
    {
        public void ValidateIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Actor ID must be greater than zero");
        }

        public void ValidateRequest(PersonRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Actor request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 2 || request.Name.Length > 100)
                throw new ValidationException("Name must be between 2 and 100 characters");

            if (!string.IsNullOrEmpty(request.Bio) && request.Bio.Length > 5000)
                throw new ValidationException("Bio cannot exceed 5000 characters");

            if (!Regex.IsMatch(request.Name, @"^[a-zA-Z\s]+$"))
                throw new ValidationException("Name should not contain special characters or numbers.");


            if (request.DOB > DateOnly.FromDateTime(DateTime.Today))
                throw new ValidationException("Date of birth cannot be in the future");

            if (request.DOB < new DateOnly(1800, 1, 1))
                throw new ValidationException("Date of birth cannot be before the year 1800");

            if (request.Gender == null)
                throw new ValidationException("Gender is required.");

            if (int.TryParse(request.Gender, out _))
            {
                throw new ValidationException("Gender must be a string value like 'Male', 'Female', or 'Other'. Numeric values are not allowed.");
            }

            if (!Enum.TryParse<Gender>(request.Gender, true, out var genderEnum) ||
                    !Enum.IsDefined(typeof(Gender), genderEnum))
            {
                throw new ValidationException($"Invalid gender value '{request.Gender}'. Allowed values are: Male, Female, Other.");
            }
        }
    }
}
