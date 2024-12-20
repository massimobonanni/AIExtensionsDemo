using AIExtensionsDemo.EmbeddingConsole.Commands.SemanticSearch;
using System.CommandLine;

namespace AIExtensionsDemo.EmbeddingConsole
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand();
            var semanticSearchCommand = new SemanticSearchCommand("semantic-search", "Make semantic search using different embedding models");
            rootCommand.Add(semanticSearchCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
