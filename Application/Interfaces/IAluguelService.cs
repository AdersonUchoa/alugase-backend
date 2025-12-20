using Application.Pagination;
using Application.Requests.Aluguel;
using Application.Responses;
using Application.Responses.Aluguel;


namespace Application.Interfaces
{
    public interface IAluguelService
    {
        Task<ApiResponse<AluguelResponse>> AddAsync(CreateAluguelRequest request);
        Task<ApiResponse<AluguelResponse>> GetByIdAsync(int id);
        Task<ApiResponse<AluguelResponse>> UpdateAsync(int id, UpdateAluguelRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAsync(int page, int limit, bool includeInactive = false, bool includeImoveis = false, bool includeInquilinos = false, string? search = null);
        Task<ApiResponse<AluguelDashboardResponse>> GetDashboardCountsAsync();
        Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAlugueisPorInquilinoIdAsync(int inquilinoId, bool includeInactive = false);
        Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAlugueisPorImovelIdAsync(int imovelId, bool includeInactive = false);
    }
}
