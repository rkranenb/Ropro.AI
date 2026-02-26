# Ropro.AI

A lightweight AI client library built on [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/microsoft-extensions-ai).

## Quick Start

1. Create a console project and install the required packages:

	```shell
	dotnet new console -n MyAiApp
	cd MyAiApp
	dotnet add package Ropro.AI
	dotnet add package Microsoft.Extensions.Hosting
	dotnet add package Microsoft.Extensions.Configuration.Json
	```

2. Add your AI configuration (for example in `appsettings.json`) so `AddAiClient` can bind `AiClientOptions`.

	```json
	{
	  "AiClient": {
	    "ApiKey": "sk-xxx",
	    "ModelName": "gpt-5.3-codex",
		"UseResponsesEndpoint": true
	  }
	}
	```

	The UseResponsesEndpoint property should be false for older (GPT-3.5/GPT-4) chat models .

3. Replace `Program.cs` with the following minimal host-based setup:

	```csharp
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Ropro.AI;

	var configuration = new ConfigurationBuilder()
	    .AddJsonFile("appsettings.json")
	    .Build();

	var services = new ServiceCollection();
	services.AddSingleton<IConfiguration>(configuration);
	services.AddAiClient();

	await using var provider = services.BuildServiceProvider();
	var client = provider.GetRequiredService<AiClient>();

	var response = await client.AskAsync("What is the capital of Assyria?");
	Console.WriteLine(response);
	```

That is enough to give consumers a running start with dependency injection in any .NET console app.

## License

MIT
