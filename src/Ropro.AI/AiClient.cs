using Microsoft.Extensions.AI;

namespace Ropro.AI;

public class AiClient(IChatClient chatClient)
{
    private readonly IChatClient _chatClient = chatClient;

    public async Task<string> AskAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var response = await _chatClient.GetResponseAsync(prompt, cancellationToken: cancellationToken);
        return response.Text ?? string.Empty;
    }
}