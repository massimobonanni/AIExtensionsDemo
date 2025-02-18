using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using System.Numerics.Tensors;
using System.Reflection;

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

#region GitHub Models
//string token = configuration["GitHubModel:Token"];
//string endpoint = configuration["GitHubModel:Endpoint"];
//string model = configuration["GitHubModel:Model"];

//var ghEmbeddingClient = new EmbeddingsClient(
//    new Uri(endpoint),
//    new AzureKeyCredential(token));
//embeddingGenerator = ghEmbeddingClient.AsEmbeddingGenerator(model);
#endregion

Console.WriteLine("Loading data from disk...");
var executableDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var dataPath = Path.Combine(executableDir, "Data", "learning.txt");
var textLines = await File.ReadAllLinesAsync(dataPath);

Console.WriteLine("Generate embedding for texts in the file...");
var textEmbeddings = await embeddingGenerator.GenerateAndZipAsync(textLines);

Console.WriteLine("Ready for questions...");

while (true)
{
    Console.Write("\nEnter a question:");
    var input = Console.ReadLine();
    if (input == "") break;

    var questionEmbedding = await embeddingGenerator.GenerateEmbeddingAsync(input);

    var results = from e in textEmbeddings
                  let similarity = TensorPrimitives.CosineSimilarity(e.Embedding.Vector.Span,questionEmbedding.Vector.Span)
                  orderby similarity descending
                  select new { Text = e.Value, Similarity = similarity};

    foreach (var result in results.Take(5))
    {
        Console.WriteLine($"\t{result.Text} ({result.Similarity:0.00})");
    }
}

