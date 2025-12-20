using Microsoft.AspNetCore.Mvc;

namespace AlugaSe.WebAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected BadRequestObjectResult Failed(string message)
        {
            return base.BadRequest(new
            {
                errors = new List<string> {
                message
            }
            });
        }

        [NonAction]
        protected ActionResult Success(object? response)
        {
            if (response is null)
                return NotFound();

            return base.Ok(response);
        }

        [NonAction]
        protected ActionResult Created(object? response)
        {
            if (response is null)
                return NotFound();

            return base.Created();
        }
    }

}
