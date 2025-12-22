using Application.Pagination;
using Application.Requests.Imovel;
using Application.Responses;
using Application.Responses.Imovel;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IImovelService
    {
        Task<ApiResponse<ImovelResponse>> AddAsync(CreateImovelRequest request);
        Task<ApiResponse<ImovelResponse>> GetByIdAsync(int id);
        Task<ApiResponse<ImovelResponse>> UpdateAsync(int id, UpdateImovelRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetAsync(int page, int limit, bool includeInactive = false, bool includeAlugueis = false, string? search = null);
        Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetByTipoAsync(TipoImovelEnum tipo, int page, int limit, bool includeInactive = false);
        Task<ApiResponse<ImovelResponse>> GetByNomeAsync(string nome, bool includeInactive = false);
        Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetImoveisComAlugueisAtivosAsync(int page, int limit);
        Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetImoveisSemAlugueisAsync(int page, int limit);
        Task<ApiResponse<ImovelDashboardResponse>> GetDashboardCountsAsync();
        Task<ApiResponse<ImovelResponse>> GetImovelByAluguelIdAsync(int aluguelId, bool includeInactive = false);
        Task<ApiResponse<DisponibilidadeImovelResponse>> CheckDisponibilidadeAsync(int imovelId, CheckDisponibilidadeImovelRequest request);
    }
}