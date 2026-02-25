using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Ropro.AI;

public static class AiClientExtensions
{
    public static IServiceCollection AddAiClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AiClientOptions>(configuration.GetSection("AiClient"));

        services.AddSingleton<IChatClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AiClientOptions>>().Value;
            return new ChatClient(options.ModelName, options.ApiKey)
                .AsIChatClient();
        });

        services.AddSingleton<AiClient>();

        return services;
    }
}