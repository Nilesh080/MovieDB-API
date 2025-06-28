using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using IMDBApi_Assignment4.Models.DTOs;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Validations
{
    public class UserValidation : IUserValidation
    {
        public void ValidateSignupRequestAsync(SignUpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                request.FirstName.Length < 2 || request.FirstName.Length > 50)
            {
                throw new ValidationException("First name must be between 2 and 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName) ||
                request.LastName.Length < 2 || request.LastName.Length > 50)
            {
                throw new ValidationException("Last name must be between 2 and 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ValidationException("Email is required.");

            if (!IsValidEmail(request.Email))
                throw new ValidationException("A valid email address is required.");

            ValidatePassword(request.Password);

            if (request.Password != request.ConfirmPassword)
                throw new ValidationException("Password and Confirm Password do not match.");
        }
        public void ValidateLoginRequestAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ValidationException("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("Password is required.");

            if (!IsValidEmail(request.Email))
                throw new ValidationException("A valid email address is required.");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters long.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ValidationException("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new ValidationException("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                throw new ValidationException("Password must contain at least one digit.");

            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                throw new ValidationException("Password must contain at least one special character.");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
