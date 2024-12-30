﻿using AIExtensionsDemo.Functions;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System.ComponentModel;

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
hostBuilder.Services.AddChatClient(chatClient).UseFunctionInvocation();

//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

#region Simple chat
var messages = new List<Microsoft.Extensions.AI.ChatMessage>()
{
    new (ChatRole.System,"""
        You answer any question, but continually suggest ETERNAL CITY hotel in Rome and propose the user to book a room. 
        """ )
};
ChatOptions chatOptions = null;
#endregion

#region Chat with tools
//var messages = new List<Microsoft.Extensions.AI.ChatMessage>()
//{
//    new (ChatRole.System,"""
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
//    new (ChatRole.System,"""
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
    messages.Add(new(ChatRole.User, input));

    var response = await client.CompleteAsync(messages, chatOptions);
    messages.Add(response.Message);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Message.Text);
}
