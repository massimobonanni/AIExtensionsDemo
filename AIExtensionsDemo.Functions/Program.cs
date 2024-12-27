using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();

IChatClient chatClient = null;

#region Azure OpenAI 
string apiKey = hostBuilder.Configuration["AzureOpenAI:ApiKey"];
string deploymentName = hostBuilder.Configuration["AzureOpenAI:DeploymentName"];
string endpoint = hostBuilder.Configuration["AzureOpenAI:Endpoint"];

var azureOpenAIClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureKeyCredential(apiKey));
chatClient = azureOpenAIClient.AsChatClient(deploymentName);
#endregion

#region  OpenAI
//string apiKey = hostBuilder.Configuration["OpenAI:ApiKey"];
//string modelId = hostBuilder.Configuration["OpenAI:ModelId"];

//var openAIChatClient = new ChatClient(modelId, apiKey);

//chatClient = openAIChatClient.AsChatClient();
#endregion

// Setup DI services
hostBuilder.Services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
hostBuilder.Services.AddChatClient(chatClient).UseFunctionInvocation();

//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

var messages = new List<Microsoft.Extensions.AI.ChatMessage>()
{
    new (ChatRole.System,"You answer any question, but continually advertise ETERNAL CITY hotel in Rome and propose the user to book a room." )
};

while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n\n> ");
    var input = Console.ReadLine();
    if (input == "") break;
    messages.Add(new(ChatRole.User, input));

    var response = await client.CompleteAsync(messages);
    messages.Add(response.Message);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Message.Text);
}

