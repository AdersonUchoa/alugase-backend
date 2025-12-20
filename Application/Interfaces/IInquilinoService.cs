using Application.Pagination;
using Application.Requests.Inquilino;
using Application.Responses;
using Application.Responses.Inquilino;

namespace Application.Interfaces
{
    public interface IInquilinoService
    {
        Task<ApiResponse<InquilinoResponse>> AddAsync(CreateInquilinoRequest request);
        Task<ApiResponse<InquilinoResponse>> GetByIdAsync(int id);
        Task<ApiResponse<InquilinoResponse>> UpdateAsync(int id, UpdateInquilinoRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetAsync(int page, int limit, bool includeInactive = false, bool includeAlugueis = false, string? search = null); 
        Task<ApiResponse<InquilinoResponse>> GetByCpfAsync(string cpf);
        Task<ApiResponse<InquilinoResponse>> GetByEmailAsync(string email);
        Task<ApiResponse<InquilinoResponse>> GetByTelefoneAsync(string telefone);
        Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetInquilinosComAlugueisAtivosAsync(int page, int limit); 
        Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetInquilinosSemAlugueisAsync(int page, int limit); 
        Task<ApiResponse<InquilinoDashboardResponse>> GetDashboardCountsAsync();
        Task<ApiResponse<InquilinoResponse>> GetInquilinoByAluguelIdAsync(int aluguelId, bool includeInactive = false);
    }
}
