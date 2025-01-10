using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
hostBuilder.Services.AddChatClient(chatClient)
    .UseFunctionInvocation();

hostBuilder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

//Run the app
var app = hostBuilder.Build();

var client = app.Services.GetRequiredService<IChatClient>();

#region Simple Image Analysis
//var message = new ChatMessage(ChatRole.User,"What is in this image?");

//var imageBytes = File.ReadAllBytes(Path.Combine("traffic-cam", "videoframe_12118.jpg"));
//message.Contents.Add(new ImageContent(imageBytes,"image/jpg"));

//var streamResponse = chatClient.CompleteStreamingAsync([message]);
//await foreach (var chunk in streamResponse)
//{
//    Console.Write(chunk.Text);
//}
//Console.WriteLine();
#endregion

#region Multi image analysis
//var imageDir=Path.Combine(AppContext.BaseDirectory, "traffic-cam");

//foreach (var imageFile in Directory.GetFiles(imageDir,"*.jpg"))
//{
//    var imageName = Path.GetFileNameWithoutExtension(imageFile);
//    var message = new ChatMessage(ChatRole.User, 
//        $"Extract information from this image from camera {imageName}");
//    message.Contents.Add(
//        new ImageContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var streamResponse = chatClient.CompleteStreamingAsync([message]);
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
//        new ImageContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var response = await chatClient.CompleteAsync<TrafficImageResult>([message]);

//    if (response.TryGetResult(out var result))
//    { 
//        Console.WriteLine($"status: {result.Status} (cars:{result.NumCars}, trucks: {result.NumTrucks})");
//    }
//}
#endregion

#region Complex image analysis with alerts
//var raiseAlertFunction = AIFunctionFactory.Create((string cameraName, string alertReason) =>
//{
//    Console.ForegroundColor = ConsoleColor.Red;
//    Console.WriteLine($"ALERT: {cameraName} - {alertReason}");
//    Console.ResetColor();
//}, "RaiseAlert");

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
//        Raise an alert only if the camera is broken or there is anything really unusual in the image.
//        """);
//    message.Contents.Add(
//        new ImageContent(File.ReadAllBytes(imageFile), "image/jpg"));

//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine($"Processing image {imageName}");
//    Console.ResetColor();

//    var response = await chatClient.CompleteAsync<TrafficImageResult>([message], chatOptions);

//    if (response.TryGetResult(out var result))
//    {
//        Console.WriteLine($"status: {result.Status} (cars:{result.NumCars}, trucks: {result.NumTrucks})");
//    }

//}
#endregion