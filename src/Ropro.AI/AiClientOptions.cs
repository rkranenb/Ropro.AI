namespace Ropro.AI;

public class AiClientOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ModelName { get; set; } = "gpt-4o-mini";
    public bool UseResponsesEndpoint { get; set; }
}