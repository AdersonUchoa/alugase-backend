using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Inquilino;
using Application.Responses;
using Application.Responses.Aluguel;
using Application.Responses.Inquilino;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class InquilinoService : IInquilinoService
    {
        private readonly IInquilinoRepository _inquilinoRepository;
        private readonly IMapper _mapper;

        public InquilinoService(IInquilinoRepository inquilinoRepository, IMapper mapper)
        {
            _inquilinoRepository = inquilinoRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<InquilinoResponse>> AddAsync(CreateInquilinoRequest request)
        {
            try
            {
                if (request.Cpf is not null)
                {
                    if (await _inquilinoRepository.ExistsByCpfAsync(request.Cpf))
                    {
                        return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Já existe um inquilino cadastrado com este CPF.", null, null);
                    }
                }

                if (request.Email is not null)
                {
                    if (await _inquilinoRepository.ExistsByEmailAsync(request.Email))
                    {
                        return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Já existe um inquilino cadastrado com este e-mail.", null, null);
                    }
                }

                var inquilino = _mapper.Map<Inquilino>(request);
                inquilino.CreatedAt = DateTime.Now;
                inquilino.IsAtivo = true;

                var created = await _inquilinoRepository.AddAsync(inquilino);
                var response = _mapper.Map<InquilinoResponse>(created);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.Created, response, "Inquilino cadastrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao cadastrar inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> GetByIdAsync(int id)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByIdAsync(id);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um inquilino inativo.", null, null);
                }

                var response = _mapper.Map<InquilinoResponse>(inquilino);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> UpdateAsync(int id, UpdateInquilinoRequest request)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByIdAsync(id);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível atualizar um inquilino inativo.", null, null);
                }

                if (request.Cpf is not null)
                {
                    if (await _inquilinoRepository.ExistsByCpfAsync(request.Cpf, id))
                    {
                        return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Já existe outro inquilino cadastrado com este CPF.", null, null);
                    }
                }

                if (request.Email is not null)
                {
                    if (await _inquilinoRepository.ExistsByEmailAsync(request.Email, id))
                    {
                        return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Já existe outro inquilino cadastrado com este e-mail.", null, null);
                    }
                }

                _mapper.Map(request, inquilino);
                inquilino.UpdatedAt = DateTime.Now;

                var updated = await _inquilinoRepository.UpdateAsync(inquilino);
                var response = _mapper.Map<InquilinoResponse>(updated);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino atualizado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao atualizar inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByIdAsync(id);

                if (inquilino == null)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Inquilino não encontrado.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.BadRequest, null, "Não é possível deletar um inquilino inativo.", null, null);
                }

                var deleted = await _inquilinoRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, deleted, "Inquilino excluído com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, false, "Erro ao excluir inquilino.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetAsync(int page, int limit, bool includeInactive = false, bool includeAlugueis = false, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                }

                var query = _inquilinoRepository.Get(includeInactive, includeAlugueis, search);

                var paginatedInquilinos = await PaginatedResult<Inquilino>.CreateAsync(query, page, limit);

                var dtos = _mapper.Map<List<InquilinoResponse>>(paginatedInquilinos.Items);

                var result = new PaginatedResult<InquilinoResponse>(dtos, paginatedInquilinos.TotalCount, paginatedInquilinos.PageIndex, paginatedInquilinos.PageSize);

                return new ApiResponse<PaginatedResult<InquilinoResponse>>(true, HttpStatusCode.OK, result, "Inquilinos obtidos com sucesso.", paginatedInquilinos.TotalPages, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<InquilinoResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilinos.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> GetByCpfAsync(string cpf)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByCpfAsync(cpf);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado com este CPF.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um inquilino inativo.", null, null);
                }

                var response = _mapper.Map<InquilinoResponse>(inquilino);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilino por CPF.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> GetByEmailAsync(string email)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByEmailAsync(email);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado com este e-mail.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um inquilino inativo.", null, null);
                }

                var response = _mapper.Map<InquilinoResponse>(inquilino);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilino por e-mail.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> GetByTelefoneAsync(string telefone)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetByTelefoneAsync(telefone);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado com este telefone.", null, null);
                }

                if (inquilino.IsAtivo == false)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um inquilino inativo.", null, null);
                }

                var response = _mapper.Map<InquilinoResponse>(inquilino);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilino por telefone.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetInquilinosComAlugueisAtivosAsync(int page, int limit)
        { 
            try
            {
                var inquilinos = await _inquilinoRepository.GetInquilinosComAlugueisAtivosAsync();

                var totalCount = inquilinos.Count;
                var paginatedList = inquilinos
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                var dtos = _mapper.Map<List<InquilinoResponse>>(paginatedList);

                var result = new PaginatedResult<InquilinoResponse>(dtos, totalCount, page, limit);

                return new ApiResponse<PaginatedResult<InquilinoResponse>>(true, HttpStatusCode.OK, result, "Inquilinos com aluguéis ativos obtidos com sucesso.", result.TotalPages, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<InquilinoResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilinos com aluguéis ativos.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<InquilinoResponse>>> GetInquilinosSemAlugueisAsync(int page, int limit)
        {
            try
            {
                var inquilinos = await _inquilinoRepository.GetInquilinosSemAlugueisAsync();

                var totalCount = inquilinos.Count;
                var paginatedList = inquilinos
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();

                var dtos = _mapper.Map<List<InquilinoResponse>>(paginatedList);

                var result = new PaginatedResult<InquilinoResponse>(dtos, totalCount, page, limit);

                return new ApiResponse<PaginatedResult<InquilinoResponse>>(true, HttpStatusCode.OK, result, "Inquilinos sem aluguéis obtidos com sucesso.", result.TotalPages, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<InquilinoResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilinos sem aluguéis.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoDashboardResponse>> GetDashboardCountsAsync()
        {
            try
            {
                var totalInquilinos = await _inquilinoRepository.GetTotalInquilinosAsync(onlyActive: true);
                var totalComAlugueis = await _inquilinoRepository.GetTotalInquilinosComAlugueisAtivosAsync();
                var totalSemAlugueis = totalInquilinos - totalComAlugueis;
                var totalInativos = await _inquilinoRepository.GetTotalInquilinosAsync(onlyActive: false) - totalInquilinos;

                var dashboard = new InquilinoDashboardResponse
                {
                    TotalInquilinos = totalInquilinos,
                    InquilinosComAlugueisAtivos = totalComAlugueis,
                    InquilinosSemAlugueis = totalSemAlugueis,
                    InquilinosInativos = totalInativos
                };

                return new ApiResponse<InquilinoDashboardResponse>(true, HttpStatusCode.OK, dashboard, "Contagens do dashboard obtidas com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoDashboardResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao obter contagens do dashboard.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<InquilinoResponse>> GetInquilinoByAluguelIdAsync(int aluguelId, bool includeInactive = false)
        {
            try
            {
                var inquilino = await _inquilinoRepository.GetInquilinoByAluguelIdAsync(aluguelId, includeInactive);

                if (inquilino == null)
                {
                    return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.NotFound, null, "Inquilino não encontrado para este aluguel.", null, null);
                }

                var response = _mapper.Map<InquilinoResponse>(inquilino);

                return new ApiResponse<InquilinoResponse>(true, HttpStatusCode.OK, response, "Inquilino encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<InquilinoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar inquilino por aluguel.", null, ex.Message);
            }
        }
    }
}
