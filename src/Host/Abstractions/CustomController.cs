using Application.ROP;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Abstractions
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CustomController(IMediator mediator) : ControllerBase
    {
        protected IMediator Mediator => mediator;

        protected ObjectResult BuildResponse<T>(Result<T> result)
        {
            var response = new Response<T>
            {
                Value = result.Value,
                Errors = result.Errors
            };

            return StatusCode((int)result.StatusCode, response);
        }

        protected ObjectResult BuildResponse(Result<Application.ROP.Unit> result)
        {
            Response<object>? response = null;

            if (!result.Succeeded)
            {
                response = new()
                {
                    Errors = result.Errors
                };
            }

            return StatusCode((int)result.StatusCode, response);
        }

        protected OkObjectResult BuildResponse<T>(T data)
        {
            var response = new Response<T>
            {
                Value = data,
            };

            return Ok(response);
        }
    }
}
