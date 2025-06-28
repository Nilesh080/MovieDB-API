using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IMDBApi_Assignment4.Services
{
    public class UserService : IUserService
    {
        private readonly IUserValidation _userValidation;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserValidation userValidation, IUserRepository userRepository, IConfiguration configuration)
        {
            _userValidation = userValidation;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> SignupAsync(SignUpRequest request)
        {
            _userValidation.ValidateSignupRequestAsync(request);
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ValidationException($"User with email '{request.Email}' already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = passwordHash
            };

            await _userRepository.CreateAsync(user);
            return "Account created successfully";
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            _userValidation.ValidateLoginRequestAsync(request);
            var existingUser = await ExistsAsync(request.Email);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                throw new ArgumentException("Invalid email or password");

            var token = GenerateJwtToken(existingUser);

            return new LoginResponse
            {
                Message = "Login successful",
                Token = token
            };
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email
            }).ToList();
        }

        public async Task<User> ExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{email}' was not found.");
            }

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:ValidIssuer"],
                Audience = _configuration["Jwt:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}