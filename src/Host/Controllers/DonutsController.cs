using Application.Commands.Donuts.Create;
using Application.Queries.Donuts;
using Application.Queries.Donuts.Dtos;
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

        /// <summary>
        /// Lista de donas
        /// </summary>
        /// <returns>Lista de donas ordenadas por Id</returns>
        [HttpGet]
        [ProducesResponseType<Response<List<DonutDto>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return BuildResponse(await Mediator.Send(new DonutsListQuery()));
        }
    }
}
