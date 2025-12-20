using Application.Interfaces;
using Application.Requests.Administrador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [Route("api/v1/administrador")]
    [Authorize]
    public class AdministradorController : BaseController
    {
        private readonly IAdministradorService _administradorService;

        public AdministradorController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        /// <summary>
        /// Cadastra um novo administrador
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateAdminRequest request)
        {
            var response = await _administradorService.AddAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista administradores com paginação e filtros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            int page = 1,
            int limit = 10,
            bool includeInactive = false,
            string? search = null)
        {
            if (page <= 0 || limit <= 0) { return BadRequest(new { success = false, message = "Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0." }); }

            var response = await _administradorService.GetAsync(page, limit, includeInactive, search);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca administrador por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _administradorService.GetByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca administrador por login
        /// </summary>
        [HttpGet("buscar-por-login/{login}")]
        public async Task<IActionResult> GetByLoginAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "O login não pode ser vazio."
                });
            }

            var response = await _administradorService.GetByLoginAsync(login);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Atualiza um administrador existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAdminRequest request)
        {
            var response = await _administradorService.UpdateAsync(id, request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Exclui (soft delete) um administrador
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _administradorService.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}