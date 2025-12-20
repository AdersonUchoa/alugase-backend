using Application.Helper;
using Application.Interfaces;
using Application.Requests.Auth;
using Application.Responses;
using Application.Responses.Auth;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAdministradorRepository administradorRepository, IConfiguration configuration)
        {
            _administradorRepository = administradorRepository;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var admin = await _administradorRepository.GetByLoginAsync(request.Login);

                if (admin is null)
                {
                    return new ApiResponse<LoginResponse>(false, HttpStatusCode.Unauthorized, null, "Login ou senha inválidos", null, null);
                }

                if (admin.IsAtivo != true)
                {
                    return new ApiResponse<LoginResponse>(false, HttpStatusCode.Forbidden, null, "Administrador deletado.", null, null);
                }

                if (!PasswordHasher.VerifyPassword(request.Senha, admin.Senha))
                {
                    return new ApiResponse<LoginResponse>(false, HttpStatusCode.Unauthorized, null, "Login ou senha inválidos", null, null);
                }

                var token = GenerateJwtToken(admin.Id, admin.Login);
                var expiresAt = DateTime.UtcNow.AddHours(8);

                var response = new LoginResponse
                {
                    Token = token,
                    Login = admin.Login,
                    ExpiresAt = expiresAt
                };

                return new ApiResponse<LoginResponse>(true, HttpStatusCode.OK, response, "Login realizado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao realizar login.", null, ex.Message);
            }
        }

        private string GenerateJwtToken(int userId, string login)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key não configurada no appsettings.json");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", userId.ToString()),
                new Claim("Login", login),
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
