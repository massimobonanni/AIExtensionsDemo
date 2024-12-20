using AIExtensionsDemo.EmbeddingConsole.Commands.SemanticSearch.OllamaSearch;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.EmbeddingConsole.Commands.SemanticSearch
{
    internal class SemanticSearchCommand : Command
    {
        public SemanticSearchCommand(string name, string? description = null) : base(name, description)
        {
            this.Add(new OllamaSearchCommand("ollama","Execute semantic search using Ollama Embedding model"));
        }
    }
}
