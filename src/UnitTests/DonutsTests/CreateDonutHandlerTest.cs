using Application.Commands.Donuts.Create;
using Application.ROP;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Abstractions;

namespace UnitTests.DonutsTests;

[TestClass]
public class CreateDonutHandlerTest : BaseTest<CreateDonutHandler>
{
    private readonly Mock<IValidator<CreateDonutCommand>> _validatorMock = new();
    private readonly Mock<ILogger<CreateDonutHandler>> _loggerMock = new();

    private CreateDonutHandler Handler => new(
        DbContext,
        _validatorMock.Object,
        _loggerMock.Object
    );

    [TestMethod]
    public async Task HappyPath()
    {
        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateDonutCommand>(), default))
            .ReturnsAsync(new ValidationResult());

        var command = new CreateDonutCommand
        {
            Name = "Frambuesa",
            Price = 19.99m
        };

        Result<int> result = await Handler.Handle(command, default);
        Assert.IsTrue(result.Succeeded, "La operación no resultó exitosa");
    }
}
