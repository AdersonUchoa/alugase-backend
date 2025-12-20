using Application.Pagination;
using Application.Requests.Administrador;
using Application.Responses;
using Application.Responses.Administrador;

namespace Application.Interfaces
{
    public interface IAdministradorService
    {
        Task<ApiResponse<AdministradorResponse>> AddAsync(CreateAdminRequest request);
        Task<ApiResponse<AdministradorResponse>> GetByIdAsync(int id);
        Task<ApiResponse<AdministradorResponse>> UpdateAsync(int id, UpdateAdminRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<PaginatedResult<AdministradorResponse>>> GetAsync(int page, int limit, bool includeInactive = false, string? search = null);
        Task<ApiResponse<AdministradorResponse>> GetByLoginAsync(string login);
        //Task<ApiResponse<AdministradorResponse>> GetAdminByTokenAsync();
    }
}
