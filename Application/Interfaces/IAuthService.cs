using Application.Requests.Auth;
using Application.Responses;
using Application.Responses.Auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
