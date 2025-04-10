using AIExtensionsDemo.TrafficAnalysis;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.Images;

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Configuration.AddUserSecrets<Program>();

IChatClient chatClient = null;

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

// Setup DI services
hostBuilder.Services.AddChatClient(chatClient)
    .UseFunctionInvocation();

hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

#region Simple Image Analysis
//var message = new ChatMessage(ChatRole.User, "What is in this image?");

//var imageBytes = File.ReadAllBytes(Path.Combine("traffic-cam", "videoframe_1000.jpg"));
//message.Contents.Add(new DataContent(imageBytes, "image/jpg"));

//var streamResponse = chatClient.GetStreamingResponseAsync([message]);
//await foreach (var chunk in streamResponse)
//{
//    Console.Write(chunk.Text);
//}
//Console.WriteLine();
#endregion

#region Multi image analysis
//var imageDir = Path.Combine(AppContext.BaseDirectory, "traffic-cam");

//foreach (var imageFile in Directory.GetFiles(imageDir, "*.jpg"))
//{
//    var imageName = Path.GetFileNameWithoutExtension(imageFile);
//    var message = new ChatMessage(ChatRole.User,
//        $"Extract information from this image from camera {imageName}");
//    message.Contents.Add(
//        new DataContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var streamResponse = chatClient.GetStreamingResponseAsync([message]);
//    await foreach (var chunk in streamResponse)
//    {
//        Console.Write(chunk.Text);
//    }
//    Console.WriteLine();
//    Console.WriteLine();
//}
#endregion

#region Complex image analysis
//var imageDir = Path.Combine(AppContext.BaseDirectory, "traffic-cam");

//foreach (var imageFile in Directory.GetFiles(imageDir, "*.jpg"))
//{
//    var imageName = Path.GetFileNameWithoutExtension(imageFile);
//    var message = new ChatMessage(ChatRole.User,
//        $"Extract information from this image from camera {imageName}");
//    message.Contents.Add(
//        new DataContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var response = await chatClient.GetResponseAsync<TrafficImageResult>([message]);

//    if (response.TryGetResult(out var result))
//    {
//        Console.WriteLine($"status: {result.Status} (cars:{result.NumCars}, trucks: {result.NumTrucks})");
//    }
//}
#endregion

#region Complex image analysis with alerts
//var raiseAlertFunction = AIFunctionFactory.Create((string cameraName, string alertReason) =>
//    {
//        Console.ForegroundColor = ConsoleColor.Red;
//        Console.WriteLine($"ALERT: {cameraName} - {alertReason}");
//        Console.ResetColor();
//    },
//    "RaiseAlert",
//    "Raise an alert for a broken image or camera, anything unusual in the image or image is modified.");

//var chatOptions = new ChatOptions()
//{
//    Tools = [raiseAlertFunction]
//};

//var imageDir = Path.Combine(AppContext.BaseDirectory, "traffic-cam");

//foreach (var imageFile in Directory.GetFiles(imageDir, "*.jpg"))
//{
//    var imageName = Path.GetFileNameWithoutExtension(imageFile);
//    var message = new ChatMessage(ChatRole.User, $$"""
//        Extract traffic information from this image from camera {{imageName}}.
//        Raise an alert only if the camera or image is broken, there is anything really unusual in the image or the image is modified adding some artifacts.
//        """);
//    message.Contents.Add(
//        new DataContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var response = await chatClient.GetResponseAsync<TrafficImageResult>([message], chatOptions);

//    if (response.TryGetResult(out var result))
//    {
//        Console.WriteLine($"status: {result.Status} (cars:{result.NumCars}, trucks: {result.NumTrucks})");
//    }
//}
#endregion