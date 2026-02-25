using Microsoft.Extensions.AI;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Ropro.AI.Tests;

public class AiClientTests
{
    [Fact]
    public async Task AskAsync_ReturnsResponseText()
    {
        // Arrange
        var chatClient = Substitute.For<IChatClient>();
        var expectedText = "Amsterdam";

        chatClient
            .GetResponseAsync(Arg.Any<IList<ChatMessage>>(), Arg.Any<ChatOptions>(), Arg.Any<CancellationToken>())
            .Returns(new ChatResponse([new ChatMessage(ChatRole.Assistant, expectedText)]));

        var aiClient = new AiClient(chatClient);

        // Act
        var actual = await aiClient.AskAsync("Wat is de hoofdstad van Nederland?");

        // Assert
        Assert.Equal(expectedText, actual);
    }

    [Fact]
    public async Task AskAsync_EmptyResponse_ReturnsEmptyString()
    {
        // Arrange
        var chatClient = Substitute.For<IChatClient>();

        chatClient
            .GetResponseAsync(Arg.Any<IList<ChatMessage>>(), Arg.Any<ChatOptions>(), Arg.Any<CancellationToken>())
            .Returns(new ChatResponse([new ChatMessage(ChatRole.Assistant, string.Empty)]));

        var aiClient = new AiClient(chatClient);

        // Act
        var actual = await aiClient.AskAsync("Wat is de hoofdstad van Nederland?");

        // Assert
        Assert.Equal(string.Empty, actual);
    }

    [Fact]
    public async Task AskAsync_WhenChatClientThrows_PropagatesException()
    {
        // Arrange
        var chatClient = Substitute.For<IChatClient>();

        chatClient
            .GetResponseAsync(Arg.Any<IList<ChatMessage>>(), Arg.Any<ChatOptions>(), Arg.Any<CancellationToken>())
            .Throws(new HttpRequestException("API unreachable"));

        var aiClient = new AiClient(chatClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            aiClient.AskAsync("Wat is de hoofdstad van Nederland?"));
    }

    [Fact]
    public async Task AskAsync_PassesPromptToChatClient()
    {
        // Arrange
        var chatClient = Substitute.For<IChatClient>();
        var prompt = "Wat is de hoofdstad van Nederland?";

        chatClient
            .GetResponseAsync(Arg.Any<IList<ChatMessage>>(), Arg.Any<ChatOptions>(), Arg.Any<CancellationToken>())
            .Returns(new ChatResponse([new ChatMessage(ChatRole.Assistant, "Amsterdam")]));

        var aiClient = new AiClient(chatClient);

        // Act
        await aiClient.AskAsync(prompt);

        // Assert
        await chatClient.Received(1).GetResponseAsync(
            Arg.Is<IList<ChatMessage>>(msgs => msgs.Any(m => m.Text == prompt)),
            Arg.Any<ChatOptions>(),
            Arg.Any<CancellationToken>());
    }
}
