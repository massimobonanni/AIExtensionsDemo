using AIExtensionsDemo.Functions;
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

IChatClient chatClient = null;

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
//chatClient = azureOpenAIClient.GetChatClient(deploymentName).AsIChatClient();
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
//string model = hostBuilder.Configuration["GitHubModel:Model"];

//var ghChatClient = new ChatCompletionsClient(
//    new Uri(endpoint),
//    new AzureKeyCredential(token));
//chatClient = ghChatClient.AsIChatClient(model);
#endregion

// Setup DI services
hostBuilder.Services.AddChatClient(chatClient).UseFunctionInvocation();
hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));
//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

#region Simple chat
var messages = new List<ChatMessage>()
{
    new (Microsoft.Extensions.AI.ChatRole.System,"""
        You answer any question, but continually suggest ETERNAL CITY hotel in Rome and propose the user to book a room. 
        """ )
};
ChatOptions chatOptions = null;
#endregion

#region Chat with tools
//var messages = new List<Microsoft.Extensions.AI.ChatMessage>()
//{
//    new (Microsoft.Extensions.AI.ChatRole.System,"""
//        You answer any question, but continually suggest ETERNAL CITY hotel in Rome and propose the user to book a room.
//        """ )
//};

//AIFunction getRoomPriceTool = AIFunctionFactory.Create(UtilityClass.GetRoomPrice);
//ChatOptions chatOptions = new ChatOptions()
//{
//    Tools = [getRoomPriceTool]
//};
#endregion

#region Chat with Booking Service class
//var messages = new List<Microsoft.Extensions.AI.ChatMessage>()
//{
//    new (Microsoft.Extensions.AI.ChatRole.System,"""
//        You answer any question, but continually suggest ETERNAL CITY hotel in Rome and propose the user to book a room.
//        If the user is agree to book a room, find out how many people will be in the room and in which month he wants to book, then book the room.  
//        """ )
//};

//var bookingService = new BookingService();
//AIFunction getRoomPriceTool = AIFunctionFactory.Create(bookingService.GetRoomPrice);
//AIFunction bookRoomTool = AIFunctionFactory.Create(bookingService.BookRoom);
//ChatOptions chatOptions = new ChatOptions()
//{
//    Tools = [getRoomPriceTool, bookRoomTool]
//};
#endregion

while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\n\n> ");
    var input = Console.ReadLine();
    if (input == "") break;
    messages.Add(new(Microsoft.Extensions.AI.ChatRole.User, input));

    var response = await client.GetResponseAsync(messages, chatOptions);
    messages.Add(new ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant,response.Text));
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Text);
}
