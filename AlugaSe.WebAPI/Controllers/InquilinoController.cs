using Application.Interfaces;
using Application.Requests.Inquilino;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [Route("api/v1/inquilino")]
    [Authorize]
    public class InquilinoController : BaseController
    {
        private readonly IInquilinoService _inquilinoService;

        public InquilinoController(IInquilinoService inquilinoService)
        {
            _inquilinoService = inquilinoService;
        }

        /// <summary>
        /// Cadastra um novo inquilino
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateInquilinoRequest request)
        {
            var response = await _inquilinoService.AddAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista inquilinos com paginação e filtros
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

            var response = await _inquilinoService.GetAsync(page, limit, includeInactive, includeAlugueis, search);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca inquilino por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _inquilinoService.GetByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca inquilino por CPF
        /// </summary>
        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpfAsync(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return Failed("O CPF não pode ser vazio.");
            }

            var response = await _inquilinoService.GetByCpfAsync(cpf);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca inquilino por e-mail
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Failed("O e-mail não pode ser vazio.");
            }

            var response = await _inquilinoService.GetByEmailAsync(email);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Busca inquilino por telefone
        /// </summary>
        [HttpGet("telefone/{telefone}")]
        public async Task<IActionResult> GetByTelefoneAsync(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
            {
                return Failed("O telefone não pode ser vazio.");
            }

            var response = await _inquilinoService.GetByTelefoneAsync(telefone);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista inquilinos com aluguéis ativos
        /// </summary>
        [HttpGet("com-alugueis-ativos")]
        public async Task<IActionResult> GetInquilinosComAlugueisAtivosAsync(int page = 1, int limit = 10)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _inquilinoService.GetInquilinosComAlugueisAtivosAsync(page, limit);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lista inquilinos sem aluguéis
        /// </summary>
        [HttpGet("sem-alugueis")]
        public async Task<IActionResult> GetInquilinosSemAlugueisAsync(int page = 1, int limit = 10)
        {
            if (page <= 0 || limit <= 0)
            {
                return Failed("Parâmetros de paginação inválidos. Page e Limit devem ser maiores que 0.");
            }

            var response = await _inquilinoService.GetInquilinosSemAlugueisAsync(page, limit);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Obtém estatísticas do dashboard de inquilinos
        /// </summary>
        [HttpGet("dashboard/counts")]
        public async Task<IActionResult> GetDashboardCountsAsync()
        {
            var response = await _inquilinoService.GetDashboardCountsAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Atualiza um inquilino existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateInquilinoRequest request)
        {
            var response = await _inquilinoService.UpdateAsync(id, request);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Exclui (soft delete) um inquilino
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _inquilinoService.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}