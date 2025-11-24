using Application.Queries.Donuts.Dtos;
using MediatR;

namespace Application.Queries.Donuts;

public class DonutsListQuery : IRequest<List<DonutDto>>
{
}
