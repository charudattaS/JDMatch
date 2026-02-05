namespace JDMatch.Application.DTOs
{
    public class RegisterRequest

    {
        public string? CompanyName { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid PlanId { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;

    }


}
