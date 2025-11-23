using Application.Commands.Donuts.Create;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        /// <summary>
        /// Crea una dona
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Id de la dona creada</returns>
        [HttpPost]
        [ProducesResponseType<Response<int>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateDonutCommand command)
        {
            return BuildResponse(await Mediator.Send(command));
        }
    }
}
