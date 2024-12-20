using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.EmbeddingConsole.Commands.SemanticSearch.OllamaSearch
{
    internal class OllamaSearchCommand : Command
    {
        public OllamaSearchCommand(string name, string? description = null) : base(name, description)
        {
        }
    }
}
