using Application.Interfaces;
using Application.Requests.Aluguel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [Route("api/v1/aluguel")]
    [Authorize]
    public class AluguelController : BaseController
    {
        private readonly IAluguelService _aluguelService;

        public AluguelController(IAluguelService aluguelService)
        {
            _aluguelService = aluguelService;
        }

        /// <summary>
        /// Cadastra um novo aluguel
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateAluguelRequest request)
        {
            var response = await _aluguelService.AddAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista aluguéis com paginação e filtros
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            int page = 1,
            int limit = 10,
            bool includeInactive = false,
            bool includeImoveis = false,
            bool includeInquilinos = false,
            string? search = null)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _aluguelService.GetAsync(page, limit, includeInactive, includeImoveis, includeInquilinos, search);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca aluguel por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _aluguelService.GetByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista aluguéis por inquilino
        /// </summary>
        [HttpGet("inquilino/{inquilinoId:int}")]
        public async Task<IActionResult> GetAlugueisPorInquilinoIdAsync(int inquilinoId, bool includeInactive = false)
        {
            var response = await _aluguelService.GetAlugueisPorInquilinoIdAsync(inquilinoId, includeInactive);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista aluguéis por imóvel
        /// </summary>
        [HttpGet("imovel/{imovelId:int}")]
        public async Task<IActionResult> GetAlugueisPorImovelIdAsync(int imovelId, bool includeInactive = false)
        {
            var response = await _aluguelService.GetAlugueisPorImovelIdAsync(imovelId, includeInactive);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Obtém estatísticas do dashboard de aluguéis
        /// </summary>
        [HttpGet("dashboard/counts")]
        public async Task<IActionResult> GetDashboardCountsAsync()
        {
            var response = await _aluguelService.GetDashboardCountsAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Atualiza um aluguel existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAluguelRequest request)
        {
            var response = await _aluguelService.UpdateAsync(id, request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Exclui (soft delete) um aluguel
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _aluguelService.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}