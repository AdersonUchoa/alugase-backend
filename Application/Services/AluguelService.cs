using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Aluguel;
using Application.Responses;
using Application.Responses.Administrador;
using Application.Responses.Aluguel;
using Application.Responses.Imovel;
using Application.Responses.Inquilino;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class AluguelService : IAluguelService
    {
        private readonly IAluguelRepository _aluguelRepository;
        private readonly IMapper _mapper;

        public AluguelService(IAluguelRepository aluguelRepository, IMapper mapper)
        {
            _aluguelRepository = aluguelRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AluguelResponse>> AddAsync(CreateAluguelRequest request)
        {
            try
            {
                var aluguel = _mapper.Map<Aluguel>(request);
                aluguel.CreatedAt = DateTime.Now;
                aluguel.IsAtivo = true;

                var created = await _aluguelRepository.AddAsync(aluguel);
                var response = _mapper.Map<AluguelResponse>(created);

                return new ApiResponse<AluguelResponse>(true, HttpStatusCode.Created, response, "Aluguel cadastrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AluguelResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao cadastrar aluguel.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AluguelResponse>> GetByIdAsync(int id)
        {
            try
            {
                var aluguel = await _aluguelRepository.GetByIdAsync(id);

                if (aluguel == null)
                {
                    return new ApiResponse<AluguelResponse>(false, HttpStatusCode.NotFound, null, "Aluguel não encontrado.", null, null);
                }

                if (aluguel.IsAtivo == false)
                {
                    return new ApiResponse<AluguelResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um aluguel inativo.", null, null);
                }

                var response = _mapper.Map<AluguelResponse>(aluguel);

                return new ApiResponse<AluguelResponse>(true, HttpStatusCode.OK, response, "Aluguel encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AluguelResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar aluguel.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AluguelResponse>> UpdateAsync(int id, UpdateAluguelRequest request)
        {
            try
            {
                var aluguel = await _aluguelRepository.GetByIdAsync(id);

                if (aluguel == null)
                {
                    return new ApiResponse<AluguelResponse>(false, HttpStatusCode.NotFound, null, "Aluguel não encontrado.", null, null);
                }

                if (aluguel.IsAtivo == false)
                {
                    return new ApiResponse<AluguelResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível atualizar um aluguel inativo.", null, null);
                }

                var novaDataInicio = request.DataInicio ?? aluguel.DataInicio;
                var novaDataFim = request.DataFim ?? aluguel.DataFim;

                if (novaDataFim < novaDataInicio)
                {
                    return new ApiResponse<AluguelResponse>(false, HttpStatusCode.BadRequest, null, "A data de saída não pode ser anterior à data de início.", null, null);
                }

                _mapper.Map(request, aluguel);
                aluguel.UpdatedAt = DateTime.Now;

                var updated = await _aluguelRepository.UpdateAsync(aluguel);
                var response = _mapper.Map<AluguelResponse>(updated);

                return new ApiResponse<AluguelResponse>(true, HttpStatusCode.OK, response, "Aluguel atualizado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AluguelResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao atualizar aluguel.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var aluguel = await _aluguelRepository.GetByIdAsync(id);

                if (aluguel == null)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Inquilino não encontrado.", null, null);
                }

                if (aluguel.IsAtivo == false)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.BadRequest, null, "Não é possível deletar um aluguel inativo.", null, null);
                }

                var deleted = await _aluguelRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, deleted, "Inquilino excluído com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, false, "Erro ao excluir inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAsync(int page, int limit, bool includeInactive = false, bool includeImoveis = true, bool includeInquilinos = true, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                }

                var query = _aluguelRepository.Get(includeInactive, includeImoveis, includeInquilinos, search);

                var paginatedAlugueis = await PaginatedResult<Aluguel>.CreateAsync(query, page, limit);

                var alugueisDto = paginatedAlugueis.Items.Select(a => new AluguelResponse
                {
                    Id = a.Id,
                    DataInicio = a.DataInicio,
                    DataFim = a.DataFim,
                    Valor = a.Valor,
                    MetodoDePagamento = a.MetodoDePagamento.Value(),
                    Status = a.Status.ToString(),
                    StatusDescricao = a.Status.Value(),
                    InquilinoId = a.InquilinoId,
                    ImovelId = a.ImovelId,
                    Inquilino = a.Inquilino != null ? new InquilinoResponse
                    {
                        Id = a.Inquilino.Id,
                        Nome = a.Inquilino.Nome
                    }
                    : null,
                    Imovel = a.Imovel != null ? new ImovelResponse
                    {
                        Id = a.Imovel.Id,
                        Nome = a.Imovel.Nome
                    }
                    : null
                }).ToList();

                var result = new PaginatedResult<AluguelResponse>(alugueisDto, paginatedAlugueis.TotalCount, paginatedAlugueis.PageIndex, paginatedAlugueis.PageSize);

                return new ApiResponse<PaginatedResult<AluguelResponse>>(true, HttpStatusCode.OK, result, "Inquilinos obtidos com sucesso.", paginatedAlugueis.TotalPages, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<AluguelResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilinos.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AluguelDashboardResponse>> GetDashboardCountsAsync()
        {
            try
            {
                var totalInquilinos = await _aluguelRepository.GetTotalAlugueisAsync(onlyActive: true);
                var alugueisEmAndamento = await _aluguelRepository.GetAlugueisPorStatusAsync(AluguelStatusesEnum.EmAndamento);
                var alugueisConcluidos = await _aluguelRepository.GetAlugueisPorStatusAsync(AluguelStatusesEnum.Finalizado);
                var alugueisCancelados = await _aluguelRepository.GetAlugueisPorStatusAsync(AluguelStatusesEnum.Cancelado);

                var dashboard = new AluguelDashboardResponse
                {
                    TotalAlugueis = totalInquilinos,
                    AlugueisEmAndamento = alugueisEmAndamento.Count,
                    AlugueisConcluídos = alugueisConcluidos.Count,
                    AlugueisCancelados = alugueisCancelados.Count
                };

                return new ApiResponse<AluguelDashboardResponse>(true, HttpStatusCode.OK, dashboard, "Contagens do dashboard obtidas com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AluguelDashboardResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao obter contagens do dashboard.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAlugueisPorInquilinoIdAsync(int inquilinoId, bool includeInactive = false)
        {
            try
            {
                var alugueis = await _aluguelRepository.GetAlugueisPorInquilinoIdAsync(inquilinoId, includeInactive);

                var dtos = _mapper.Map<List<AluguelResponse>>(alugueis);

                var result = new PaginatedResult<AluguelResponse>(dtos, dtos.Count, 1, dtos.Count);

                return new ApiResponse<PaginatedResult<AluguelResponse>>(true, HttpStatusCode.OK, result, "Aluguéis do inquilino obtidos com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<AluguelResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar aluguéis do inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<AluguelResponse>>> GetAlugueisPorImovelIdAsync(int imovelId, bool includeInactive = false)
        {
            try
            {
                var alugueis = await _aluguelRepository.GetAlugueisPorImovelIdAsync(imovelId, includeInactive);

                var dtos = _mapper.Map<List<AluguelResponse>>(alugueis);

                var result = new PaginatedResult<AluguelResponse>(dtos, dtos.Count, 1, dtos.Count);

                return new ApiResponse<PaginatedResult<AluguelResponse>>(true, HttpStatusCode.OK, result, "Aluguéis do imóvel obtidos com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<AluguelResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar aluguéis do imóvel.", null, ex.Message);
            }
        }
    }
}
