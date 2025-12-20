using Application.Helper;
using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Administrador;
using Application.Responses;
using Application.Responses.Administrador;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Application.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IMapper _mapper;

        public AdministradorService(IAdministradorRepository administradorRepository, IMapper mapper)
        {
            _administradorRepository = administradorRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AdministradorResponse>> AddAsync(CreateAdminRequest request)
        {
            try
            {
                var exists = await _administradorRepository.ExistsByLoginAsync(request.Login);
                if (exists)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.Conflict, null, "Já existe um administrador com este login.", null, null);
                }

                var admin = _mapper.Map<Administrador>(request);
                admin.Senha = PasswordHasher.HashPassword(request.Senha);
                admin.CreatedAt = DateTime.Now;
                admin.IsAtivo = true;

                var created = await _administradorRepository.AddAsync(admin);
                var response = _mapper.Map<AdministradorResponse>(created);

                return new ApiResponse<AdministradorResponse>(true, HttpStatusCode.Created, response, "Administrador cadastrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao cadastrar administrador.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AdministradorResponse>> GetByIdAsync(int id)
        {
            try
            {
                var admin = await _administradorRepository.GetByIdAsync(id);

                if (admin == null)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.NotFound, null, "Administrador não encontrado.", null, null);
                }

                if (admin.IsAtivo == false)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um administrador inativo.", null, null);
                }

                var response = _mapper.Map<AdministradorResponse>(admin);

                return new ApiResponse<AdministradorResponse>(true, HttpStatusCode.OK, response, "Administrador encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar administrador.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AdministradorResponse>> UpdateAsync(int id, UpdateAdminRequest request)
        {
            try
            {
                var admin = await _administradorRepository.GetByIdAsync(id);

                if (admin == null)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.NotFound, null, "Administrador não encontrado.", null, null);
                }

                if (admin.IsAtivo == false)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível atualizar um administrador inativo.", null, null);
                }

                var existsLogin = await _administradorRepository.ExistsByLoginAsync(request.Login, id);
                if (existsLogin)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.Conflict, null, "Já existe um administrador com este login.", null, null);
                }

                _mapper.Map(request, admin);

                if (request.Senha is not null)
                {
                    admin.Senha = PasswordHasher.HashPassword(request.Senha);
                }

                admin.UpdatedAt = DateTime.Now;

                var updated = await _administradorRepository.UpdateAsync(admin);
                var response = _mapper.Map<AdministradorResponse>(updated);

                return new ApiResponse<AdministradorResponse>(true, HttpStatusCode.OK, response, "Administrador atualizado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao atualizar administrador.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var admin = await _administradorRepository.GetByIdAsync(id);

                if (admin == null)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Administrador não encontrado.", null, null);
                }

                if (admin.IsAtivo == false)
                {
                    return new ApiResponse<bool>(false, HttpStatusCode.BadRequest, null, "Não é possível deletar um administrador inativo.", null, null);
                }

                var deleted = await _administradorRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, deleted, "Administrador excluído com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, false, "Erro ao excluir administrador.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<AdministradorResponse>>> GetAsync(int page, int limit, bool includeInactive = false, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                }

                var query = _administradorRepository.Get(includeInactive, search);

                var paginatedAdmins = await PaginatedResult<Administrador>.CreateAsync(query, page, limit);

                var dtos = _mapper.Map<List<AdministradorResponse>>(paginatedAdmins.Items);

                var result = new PaginatedResult<AdministradorResponse>(dtos, paginatedAdmins.TotalCount, paginatedAdmins.PageIndex, paginatedAdmins.PageSize);

                return new ApiResponse<PaginatedResult<AdministradorResponse>>(true, HttpStatusCode.OK, result, "Administradores obtidos com sucesso.", paginatedAdmins.TotalPages, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<AdministradorResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar administradores.", null, ex.Message);
            }
        }

        public async Task<ApiResponse<AdministradorResponse>> GetByLoginAsync(string login)
        {
            try
            {
                var admin = await _administradorRepository.GetByLoginAsync(login);

                if (admin == null)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.NotFound, null, "Administrador não encontrado com este CPF.", null, null);
                }

                if (admin.IsAtivo == false)
                {
                    return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.BadRequest, null, "Não é possível obter um administrador inativo.", null, null);
                }

                var response = _mapper.Map<AdministradorResponse>(admin);

                return new ApiResponse<AdministradorResponse>(true, HttpStatusCode.OK, response, "Administrador encontrado com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AdministradorResponse>(false, HttpStatusCode.InternalServerError, null, "Erro ao buscar administrador por login.", null, ex.Message);
            }
        }
    }
}
