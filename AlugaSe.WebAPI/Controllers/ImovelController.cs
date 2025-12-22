using Application.Interfaces;
using Application.Requests.Imovel;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [Route("api/v1/imovel")]
    [Authorize]
    public class ImovelController : BaseController
    {
        private readonly IImovelService _imovelService;

        public ImovelController(IImovelService imovelService)
        {
            _imovelService = imovelService;
        }

        /// <summary>
        /// Cadastra um novo imóvel
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateImovelRequest request)
        {
            var response = await _imovelService.AddAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista imóveis com paginação e filtros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            int page = 1,
            int limit = 10,
            bool includeInactive = false,
            bool includeAlugueis = false,
            string? search = null)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _imovelService.GetAsync(page, limit, includeInactive, includeAlugueis, search);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca imóvel por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _imovelService.GetByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca imóvel por tipo
        /// </summary>
        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> GetByTipoAsync(TipoImovelEnum tipo, int page = 1, int limit = 10, bool includeInactive = false)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _imovelService.GetByTipoAsync(tipo, page, limit, includeInactive);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca imóvel por nome
        /// </summary>
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetByNomeAsync(string nome, bool includeInactive = false)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return Failed("O nome do imóvel não pode ser vazio.");
            }

            var response = await _imovelService.GetByNomeAsync(nome, includeInactive);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista imóveis com aluguéis ativos
        /// </summary>
        [HttpGet("com-alugueis-ativos")]
        public async Task<IActionResult> GetImoveisComAlugueisAtivosAsync(int page = 1, int limit = 10)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _imovelService.GetImoveisComAlugueisAtivosAsync(page, limit);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista imóveis sem aluguéis (disponíveis)
        /// </summary>
        [HttpGet("sem-alugueis")]
        public async Task<IActionResult> GetImoveisSemAlugueisAsync(int page = 1, int limit = 10)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _imovelService.GetImoveisSemAlugueisAsync(page, limit);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Obtém estatísticas do dashboard de imóveis
        /// </summary>
        [HttpGet("dashboard/counts")]
        public async Task<IActionResult> GetDashboardCountsAsync()
        {
            var response = await _imovelService.GetDashboardCountsAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Atualiza um imóvel existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateImovelRequest request)
        {
            var response = await _imovelService.UpdateAsync(id, request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Exclui (soft delete) um imóvel
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _imovelService.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Verifica a disponibilidade de um imóvel em uma data específica
        /// </summary>
        [HttpPost("{id:int}/verificar-disponibilidade")]
        public async Task<IActionResult> CheckDisponibilidadeAsync(int id, [FromBody] CheckDisponibilidadeImovelRequest request)
        {
            if (request.Data < DateOnly.FromDateTime(DateTime.Today))
            {
                return Failed("Não é possível verificar disponibilidade para datas passadas.");
            }

            var response = await _imovelService.CheckDisponibilidadeAsync(id, request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}