using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Imovel;
using Application.Responses;
using Application.Responses.Imovel;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class ImovelService : IImovelService
    {
        private readonly IImovelRepository _imovelRepository;
        private readonly IMapper _mapper;

        public ImovelService(IImovelRepository imovelRepository, IMapper mapper)
        {
            _imovelRepository = imovelRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ImovelResponse>> AddAsync(CreateImovelRequest request)
        {
            try
            {
                if (await _imovelRepository.ExistsByNomeAsync(request.Nome))
                {
                    return new ApiResponse<ImovelResponse>(false, HttpStatusCode.BadRequest, null, "Já existe um imovel cadastrado com este nome.", null, null);
                }

                var imovel = _mapper.Map<Imovel>(request);
                imovel.CreatedAt = DateTime.Now;
                imovel.IsAtivo = true;

                var created = await _imovelRepository.AddAsync(imovel);
                var response = _mapper.Map<ImovelResponse>(created);

                return new ApiResponse<ImovelResponse>(
                    true,
                    HttpStatusCode.Created,
                    response,
                    "Imóvel cadastrado com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao cadastrar imóvel.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<ImovelResponse>> GetByIdAsync(int id)
        {
            try
            {
                var imovel = await _imovelRepository.GetByIdAsync(id);

                if (imovel == null)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.NotFound,
                        null,
                        "Imóvel não encontrado.",
                        null,
                        null
                    );
                }

                if(imovel.IsAtivo == false)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.BadRequest,
                        null,
                        "Não é possível obter um imóvel inativo.",
                        null,
                        null
                    );
                }

                var response = _mapper.Map<ImovelResponse>(imovel);

                return new ApiResponse<ImovelResponse>(
                    true,
                    HttpStatusCode.OK,
                    response,
                    "Imóvel encontrado com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóvel.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<ImovelResponse>> UpdateAsync(int id, UpdateImovelRequest request)
        {
            try
            {
                if (request.Nome is not null)
                {
                    if (await _imovelRepository.ExistsByNomeAsync(request.Nome, id))
                    {
                        return new ApiResponse<ImovelResponse>(false, HttpStatusCode.BadRequest, null, "Já existe outro imóvel cadastrado com este nome.", null, null);
                    }
                }

                var imovel = await _imovelRepository.GetByIdAsync(id);

                if (imovel == null)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.NotFound,
                        null,
                        "Imóvel não encontrado.",
                        null,
                        null
                    );
                }

                if (imovel.IsAtivo == false)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.BadRequest,
                        null,
                        "Não é possível atualizar um imóvel inativo.",
                        null,
                        null
                    );
                }

                _mapper.Map(request, imovel);
                imovel.UpdatedAt = DateTime.Now;

                var updated = await _imovelRepository.UpdateAsync(imovel);
                var response = _mapper.Map<ImovelResponse>(updated);

                return new ApiResponse<ImovelResponse>(
                    true,
                    HttpStatusCode.OK,
                    response,
                    "Imóvel atualizado com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao atualizar imóvel.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var imovel = await _imovelRepository.GetByIdAsync(id);

                if (imovel == null)
                {
                    return new ApiResponse<bool>(
                        false,
                        HttpStatusCode.NotFound,
                        false,
                        "Imóvel não encontrado.",
                        null,
                        null
                    );
                }

                if (imovel.IsAtivo == false)
                {
                    return new ApiResponse<bool>(
                        false,
                        HttpStatusCode.BadRequest,
                        null,
                        "Não é possível deletar um imóvel inativo.",
                        null,
                        null
                    );
                }

                var deleted = await _imovelRepository.DeleteAsync(id);

                return new ApiResponse<bool>(
                    true,
                    HttpStatusCode.OK,
                    deleted,
                    "Imóvel excluído com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(
                    false,
                    HttpStatusCode.InternalServerError,
                    false,
                    "Erro ao excluir imóvel.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetAsync(
            int page,
            int limit,
            bool includeInactive = false,
            bool includeAlugueis = true,
            string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                }

                var query = _imovelRepository.Get(includeInactive, includeAlugueis, search);

                var paginatedImoveis = await PaginatedResult<Imovel>.CreateAsync(query, page, limit);

                var dtos = _mapper.Map<List<ImovelResponse>>(paginatedImoveis.Items);

                var result = new PaginatedResult<ImovelResponse>(
                    dtos,
                    paginatedImoveis.TotalCount,
                    paginatedImoveis.PageIndex,
                    paginatedImoveis.PageSize
                );

                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    true,
                    HttpStatusCode.OK,
                    result,
                    "Imóveis obtidos com sucesso.",
                    paginatedImoveis.TotalPages,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóveis.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetByTipoAsync(
            TipoImovelEnum tipo,
            int page,
            int limit,
            bool includeInactive = false)
        {
            try
            {
                var imoveis = await _imovelRepository.GetByTipoAsync(tipo, includeInactive);

                if (imoveis == null || !imoveis.Any())
                {
                    return new ApiResponse<PaginatedResult<ImovelResponse>>(
                        false,
                        HttpStatusCode.NotFound,
                        null,
                        "Nenhum imóvel encontrado com este tipo.",
                        null,
                        null
                    );
                }

                var totalCount = imoveis.Count;
                var paginatedList = imoveis
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                var dtos = _mapper.Map<List<ImovelResponse>>(paginatedList);

                var result = new PaginatedResult<ImovelResponse>(
                    dtos,
                    totalCount,
                    page,
                    limit
                );

                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    true,
                    HttpStatusCode.OK,
                    result,
                    $"Imóveis do tipo '{tipo}' obtidos com sucesso.",
                    result.TotalPages,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóveis por tipo.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetImoveisComAlugueisAtivosAsync(int page, int limit)
        {
            try
            {
                var imoveis = await _imovelRepository.GetImoveisComAlugueisAtivosAsync();

                var totalCount = imoveis.Count;
                var paginatedList = imoveis
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                var dtos = _mapper.Map<List<ImovelResponse>>(paginatedList);

                var result = new PaginatedResult<ImovelResponse>(
                    dtos,
                    totalCount,
                    page,
                    limit
                );

                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    true,
                    HttpStatusCode.OK,
                    result,
                    "Imóveis com aluguéis ativos obtidos com sucesso.",
                    result.TotalPages,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóveis com aluguéis ativos.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<PaginatedResult<ImovelResponse>>> GetImoveisSemAlugueisAsync(int page, int limit)
        {
            try
            {
                var imoveis = await _imovelRepository.GetImoveisSemAlugueisAsync();

                var totalCount = imoveis.Count;
                var paginatedList = imoveis
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                var dtos = _mapper.Map<List<ImovelResponse>>(paginatedList);

                var result = new PaginatedResult<ImovelResponse>(
                    dtos,
                    totalCount,
                    page,
                    limit
                );

                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    true,
                    HttpStatusCode.OK,
                    result,
                    "Imóveis sem aluguéis obtidos com sucesso.",
                    result.TotalPages,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<ImovelResponse>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóveis sem aluguéis.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<ImovelDashboardResponse>> GetDashboardCountsAsync()
        {
            try
            {
                var totalImoveis = await _imovelRepository.GetTotalImoveisAsync(onlyActive: true);
                var totalComAlugueis = await _imovelRepository.GetTotalImoveisComAlugueisAtivosAsync();
                var totalSemAlugueis = totalImoveis - totalComAlugueis;
                var totalInativos = await _imovelRepository.GetTotalImoveisAsync(onlyActive: false) - totalImoveis;

                var dashboard = new ImovelDashboardResponse
                {
                    TotalImoveis = totalImoveis,
                    ImoveisComAlugueisAtivos = totalComAlugueis,
                    ImoveisSemAlugueis = totalSemAlugueis,
                    ImoveisInativos = totalInativos
                };

                return new ApiResponse<ImovelDashboardResponse>(
                    true,
                    HttpStatusCode.OK,
                    dashboard,
                    "Contagens do dashboard obtidas com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelDashboardResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao obter contagens do dashboard.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<ImovelResponse>> GetImovelByAluguelIdAsync(int aluguelId, bool includeInactive = false)
        {
            try
            {
                var imovel = await _imovelRepository.GetImovelByAluguelIdAsync(aluguelId, includeInactive);

                if (imovel == null)
                {
                    return new ApiResponse<ImovelResponse>(false, HttpStatusCode.NotFound, null, "Imovel não encontrado para este aluguel.", null, null);
                }

                var response = _mapper.Map<ImovelResponse>(imovel);

                return new ApiResponse<ImovelResponse>(true, HttpStatusCode.OK, response, "Imovel encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar imovel por aluguel.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<ImovelResponse>> GetByNomeAsync(string nome, bool includeInactive = false)
        {
            try
            {
                var imovel = await _imovelRepository.GetByNomeAsync(nome, includeInactive);
                
                if (imovel == null)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.NotFound,
                        null,
                        "Imóvel não encontrado.",
                        null,
                        null
                    );
                }

                if (imovel.IsAtivo == false)
                {
                    return new ApiResponse<ImovelResponse>(
                        false,
                        HttpStatusCode.BadRequest,
                        null,
                        "Não é possível deletar um imóvel inativo.",
                        null,
                        null
                    );
                }

                var response = _mapper.Map<ImovelResponse>(imovel);
                return new ApiResponse<ImovelResponse>(
                    true,
                    HttpStatusCode.OK,
                    response,
                    "Imóvel encontrado com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImovelResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao buscar imóvel por nome.",
                    null,
                    ex.Message
                );
            }
        }

        public async Task<ApiResponse<DisponibilidadeImovelResponse>> CheckDisponibilidadeAsync(int imovelId, CheckDisponibilidadeImovelRequest request)
        {
            try
            {
                var imovel = await _imovelRepository.GetByIdAsync(imovelId);

                if (imovel == null)
                {
                    return new ApiResponse<DisponibilidadeImovelResponse>(false, HttpStatusCode.NotFound, null, "Imóvel não encontrado.", null, null);
                }

                if (imovel.IsAtivo == false)
                {
                    return new ApiResponse<DisponibilidadeImovelResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível verificar disponibilidade de um imóvel inativo.", null, null);
                }

                var isDisponivel = await _imovelRepository.IsDisponivelAsync(imovelId, request.Data);

                var response = new DisponibilidadeImovelResponse
                {
                    ImovelId = imovelId,
                    NomeImovel = imovel.Nome,
                    DataConsultada = request.Data,
                    EstaDisponivel = isDisponivel
                };

                if (isDisponivel)
                {
                    response.Mensagem = $"O imóvel '{imovel.Nome}' está disponível para a data {request.Data:dd/MM/yyyy}.";
                }
                else
                {
                    // Busca informações do aluguel conflitante
                    var aluguelConflitante = imovel.Aluguels?
                        .FirstOrDefault(a =>
                            a.IsAtivo == true &&
                            a.Status == AluguelStatusesEnum.EmAndamento &&
                            a.DataInicio <= request.Data &&
                            a.DataFim >= request.Data);

                    if (aluguelConflitante != null)
                    {
                        response.AluguelConflitante = new AluguelConflitanteInfo
                        {
                            AluguelId = aluguelConflitante.Id,
                            DataInicio = aluguelConflitante.DataInicio,
                            DataFim = aluguelConflitante.DataFim,
                            NomeInquilino = aluguelConflitante.Inquilino?.Nome
                        };

                        response.Mensagem = $"O imóvel '{imovel.Nome}' NÃO está disponível para a data {request.Data:dd/MM/yyyy}. " +
                            $"Existe um aluguel ativo de {aluguelConflitante.DataInicio:dd/MM/yyyy} até {aluguelConflitante.DataFim:dd/MM/yyyy}.";
                    }
                    else
                    {
                        response.Mensagem = $"O imóvel '{imovel.Nome}' NÃO está disponível para a data {request.Data:dd/MM/yyyy}.";
                    }
                }

                return new ApiResponse<DisponibilidadeImovelResponse>(
                    true,
                    HttpStatusCode.OK,
                    response,
                    "Verificação de disponibilidade realizada com sucesso.",
                    null,
                    null
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<DisponibilidadeImovelResponse>(
                    false,
                    HttpStatusCode.InternalServerError,
                    null,
                    "Erro ao verificar disponibilidade do imóvel.",
                    null,
                    ex.Message
                );
            }
        }
    }
}