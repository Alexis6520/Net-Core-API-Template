using Application.Queries.Donuts.Dtos;
using Domain.Services.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Donuts;

public class DonutQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<DonutsListQuery, List<DonutDto>>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<DonutDto>> Handle(DonutsListQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Donuts
            .OrderBy(x => x.Id)
            .Select(x => new DonutDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            })
            .ToListAsync(cancellationToken);

        return list;
    }
}
