using Domain.Enums;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [Route("api/v1/enums")]
    [Authorize]
    public class EnumsController : BaseController
    {
        [HttpGet("tipos-imovel")]
        public IActionResult GetTiposImovel()
        {
            var tipos = Enum.GetValues<TipoImovelEnum>()
                .Select(t => new {
                    value = t.ToString(),
                    label = t.Value()
                })
                .ToList();

            return Ok(new { success = true, data = tipos });
        }

        [HttpGet("metodos-pagamento")]
        public IActionResult GetMetodosPagamento()
        {
            var metodos = Enum.GetValues<MetodoPagamentoEnum>()
                .Select(m => new {
                    value = m.ToString(),
                    label = m.Value()
                })
                .ToList();

            return Ok(new { success = true, data = metodos });
        }

        [HttpGet("status-aluguel")]
        public IActionResult GetStatusAluguel()
        {
            var status = Enum.GetValues<AluguelStatusesEnum>()
                .Select(s => new {
                    value = s.ToString(),
                    label = s.Value()
                })
                .ToList();

            return Ok(new { success = true, data = status });
        }
    }
}