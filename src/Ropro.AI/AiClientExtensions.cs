using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAI.Responses;

namespace Ropro.AI;

public static class AiClientExtensions
{
    public static IServiceCollection AddAiClient(this IServiceCollection services)
    {
        services.AddOptions<AiClientOptions>()
            .BindConfiguration("AiClient");

        services.AddSingleton<IChatClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AiClientOptions>>().Value;

            if (options.UseResponsesEndpoint)
            {
#pragma warning disable OPENAI001
                return new ResponsesClient(options.ModelName, options.ApiKey)
                    .AsIChatClient();
#pragma warning restore OPENAI001
            }

            return new ChatClient(options.ModelName, options.ApiKey)
                .AsIChatClient();
        });

        services.AddSingleton<AiClient>();

        return services;
    }
}