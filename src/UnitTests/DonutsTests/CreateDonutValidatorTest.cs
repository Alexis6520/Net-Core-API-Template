using Application.Commands.Donuts.Create;

namespace UnitTests.DonutsTests;

[TestClass]
public sealed class CreateDonutValidatorTest
{
    private readonly CreateDonutValidator _validator = new();

    public static IEnumerable<object[]> Data =>
    [
        [new CreateDonutCommand {
            Name=string.Empty,
            Price=2.5m
        }],
        [new CreateDonutCommand {
            Name=new string('A',51),
            Description=new string('A',101),
            Price=2.5m
        }],
        [new CreateDonutCommand {
            Name="Frambuesa",
            Price=0
        }],
        [new CreateDonutCommand {
            Name="Frambuesa",
            Price=-0.01m
        }]
    ];

    [TestMethod]
    [DynamicData(nameof(Data))]
    public async Task Validate(CreateDonutCommand command)
    {
        var result = await _validator.ValidateAsync(command,default);
        Assert.IsFalse(result.IsValid);
    }
}
