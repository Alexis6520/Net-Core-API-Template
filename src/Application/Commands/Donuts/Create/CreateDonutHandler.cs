using Application.Extensions;
using Application.ROP;
using Domain.Entities;
using Domain.Services.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Commands.Donuts.Create;

public class CreateDonutHandler(
    ApplicationDbContext dbContext,
    IValidator<CreateDonutCommand> validator,
    ILogger<CreateDonutHandler> logger
) : IRequestHandler<CreateDonutCommand, Result<int>>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IValidator<CreateDonutCommand> _validator = validator;
    private readonly ILogger<CreateDonutHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
    {
        return await _validator.ValidateAndMap(request)
            .Map(Create)
            .WithStatusCode(HttpStatusCode.Created);
    }

    private async Task<int> Create(CreateDonutCommand request) 
    {
        var donut = new Donut
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        _dbContext.Donuts.Add(donut);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Donut created with ID {DonutId}", donut.Id);
        return donut.Id;
    }
}
