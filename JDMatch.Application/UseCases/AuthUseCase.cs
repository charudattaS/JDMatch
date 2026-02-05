using JDMatch.Application.DTOs;
using JDMatch.Application.Implementation;
using JDMatch.Application.Interfaces;
using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Application.UseCases
{
    public class AuthUseCase
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthUseCase(
            IApplicationDbContext context,
            IJwtTokenService jwtService,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.FullName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                PlanId = request.PlanId,
                CompanyName = "",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow,
                NextBillingDate = DateTime.UtcNow.AddMonths(1),
                IsSubscriptionActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Token = token
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var hashedPassword = _passwordHasher.Hash(request.Password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.PasswordHash == hashedPassword);

            if (user == null)
                throw new Exception("Invalid credentials");

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Token = token
            };
        }
    }
}
