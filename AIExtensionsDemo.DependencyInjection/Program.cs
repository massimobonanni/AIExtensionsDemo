using AIExtensionsDemo.DependencyInjection;
using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();

IChatClient? chatClient = null;

#region Ollama (local)
//chatClient = new OllamaChatClient(
//  new Uri("http://127.0.0.1:11434"),
//  modelId: "llama3.1");
#endregion

#region Azure OpenAI 
//string apiKey = hostBuilder.Configuration["AzureOpenAI:ApiKey"];
//string deploymentName = hostBuilder.Configuration["AzureOpenAI:DeploymentName"];
//string endpoint = hostBuilder.Configuration["AzureOpenAI:Endpoint"];

//var azureOpenAIClient = new AzureOpenAIClient(
//    new Uri(endpoint),
//    new AzureKeyCredential(apiKey));
//chatClient = azureOpenAIClient.AsChatClient(deploymentName);
#endregion

#region  OpenAI
//string apiKey = hostBuilder.Configuration["OpenAI:ApiKey"];
//string modelId = hostBuilder.Configuration["OpenAI:ModelId"];

//var openAIChatClient = new ChatClient(modelId, apiKey);

//chatClient = openAIChatClient.AsChatClient();
#endregion

#region GitHub Models
//string token = hostBuilder.Configuration["GitHubModel:Token"];
//string endpoint = hostBuilder.Configuration["GitHubModel:Endpoint"];
//string model = hostBuilder.Configuration["GitHubModel:Model"];

//var ghChatClient = new ChatCompletionsClient(
//    new Uri(endpoint),
//    new AzureKeyCredential(token));
//chatClient = ghChatClient.AsChatClient(model);
#endregion

#region Loren Ipsum Chat Client
//chatClient = new LorenIpsumChatClient();
#endregion

// Setup DI services
hostBuilder.Services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
hostBuilder.Services.AddChatClient(chatClient);

//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

#region Sync response
//var response = await client.CompleteAsync("What is a Generative Model?");
//Console.WriteLine(response.Message.Text);
#endregion

#region OpenAI Features
//if (response.RawRepresentation is OpenAI.Chat.ChatCompletion openAICompletion)
//{
//    Console.WriteLine($"Model: {openAICompletion.Model}");
//    Console.WriteLine($"Usage: InputTokenCount={openAICompletion.Usage.InputTokenCount}, OutputTokenCount={openAICompletion.Usage.OutputTokenCount}");
//}
#endregion

#region Stream response
var streamResponse = chatClient.CompleteStreamingAsync("What is a Generative Model?");
await foreach (var chunk in streamResponse)
{
    Console.Write(chunk.Text);
}
Console.WriteLine();
#endregion

