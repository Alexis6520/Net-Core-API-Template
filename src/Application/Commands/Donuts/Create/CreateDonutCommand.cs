using Application.ROP;
using MediatR;

namespace Application.Commands.Donuts.Create;

public class CreateDonutCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
