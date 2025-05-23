﻿using AIExtensionsDemo.Middleware;
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
//    new Uri("http://127.0.0.1:11434"),
//    modelId: "deepseek-r1:7b");
#endregion

#region Azure OpenAI 
string apiKey = hostBuilder.Configuration["AzureOpenAI:ApiKey"];
string deploymentName = hostBuilder.Configuration["AzureOpenAI:DeploymentName"];
string endpoint = hostBuilder.Configuration["AzureOpenAI:Endpoint"];

var azureOpenAIClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureKeyCredential(apiKey));
chatClient = azureOpenAIClient.GetChatClient(deploymentName).AsIChatClient();
#endregion

#region  OpenAI
//string apiKey = hostBuilder.Configuration["OpenAI:ApiKey"];
//string modelId = hostBuilder.Configuration["OpenAI:ModelId"];

//var openAIChatClient = new OpenAI.Chat.ChatClient(modelId, apiKey);

//chatClient = openAIChatClient.AsIChatClient();
#endregion

#region GitHub Models
//string token = hostBuilder.Configuration["GitHubModel:Token"];
//string endpoint = hostBuilder.Configuration["GitHubModel:Endpoint"];
//string model = hostBuilder.Configuration["GitHubModel:Model"]; // DeepSeek-V3

//var ghChatClient = new ChatCompletionsClient(
//    new Uri(endpoint),
//    new AzureKeyCredential(token));
//chatClient = ghChatClient.AsIChatClient(model);
#endregion

// Setup DI services
hostBuilder.Services.AddChatClient(chatClient)
    .UseLanguage("italian")
    //.UseRateLimit(TimeSpan.FromSeconds(30))
    //.UseTokenCounter()
    .UseFunctionInvocation();

hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));
//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

var messages = new List<ChatMessage>()
{
    new (Microsoft.Extensions.AI.ChatRole.System,"""
        You are an intelligent agent that answer any question. 
        """ )
};

while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n\n> ");
    var input = Console.ReadLine();
    if (input == "") break;
    messages.Add(new(Microsoft.Extensions.AI.ChatRole.User, input));

    var response = await client.GetResponseAsync(messages);
    messages.Add(new ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant,response.Text));
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Text);
}