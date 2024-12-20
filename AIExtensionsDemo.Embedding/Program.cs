using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Embeddings;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = null;

#region Ollama allminilm (local)
//embeddingGenerator = new OllamaEmbeddingGenerator(
//   new Uri("http://127.0.0.1:11434"),
//   modelId: "all-minilm");
#endregion

#region Azure OpenAI 
//string apiKey = configuration["AzureOpenAI:ApiKey"];
//string deploymentName = configuration["AzureOpenAI:DeploymentName"];
//string endpoint = configuration["AzureOpenAI:Endpoint"];

//var azureOpenAIClient = new AzureOpenAIClient(
//    new Uri(endpoint), 
//    new AzureKeyCredential(apiKey));
//embeddingGenerator = azureOpenAIClient.AsEmbeddingGenerator(deploymentName); 
#endregion

#region  OpenAI
//string apiKey = configuration["OpenAI:ApiKey"];
//string modelId = configuration["OpenAI:ModelId"];

//var embeddingClient = new EmbeddingClient(modelId,apiKey);

//embeddingGenerator = new OpenAIEmbeddingGenerator(embeddingClient);
#endregion

var embedding = await embeddingGenerator.GenerateEmbeddingAsync("Hello, World!");
Console.WriteLine($"Vector lenght {embedding.Vector.Length}");
foreach (var value in embedding.Vector.Span)
{
    Console.Write($"{value:0.00} ");
}

