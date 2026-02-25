using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ropro.AI.Tests;

public class AiClientOptionsTests
{
    [Fact]
    public void AiClientOptions_BindsFromConfiguration()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AiClient:ApiKey"] = "test-api-key",
                ["AiClient:ModelName"] = "gpt-4o"
            })
            .Build();

        var services = new ServiceCollection();
        services.Configure<AiClientOptions>(config.GetSection("AiClient"));

        var sp = services.BuildServiceProvider();
        var options = sp.GetRequiredService<IOptions<AiClientOptions>>().Value;

        // Assert
        Assert.Equal("test-api-key", options.ApiKey);
        Assert.Equal("gpt-4o", options.ModelName);
    }

    [Fact]
    public void AiClientOptions_MissingSection_UsesDefaults()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var services = new ServiceCollection();
        services.Configure<AiClientOptions>(config.GetSection("AiClient"));

        var sp = services.BuildServiceProvider();
        var options = sp.GetRequiredService<IOptions<AiClientOptions>>().Value;

        // Assert
        Assert.Equal(string.Empty, options.ApiKey);
        Assert.Equal("gpt-4o-mini", options.ModelName);
    }
}