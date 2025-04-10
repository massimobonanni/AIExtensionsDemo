using Azure;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.Middleware
{
    internal static class UseTokenCounterStep
    {
        public static ChatClientBuilder UseTokenCounter(this ChatClientBuilder builder)
            => builder.Use(inner => new TokenCounterChatClient(inner));

        private class TokenCounterChatClient(IChatClient inner) : DelegatingChatClient(inner)
        {

            private static int TotalInputTokens = 0;
            private static int TotalOutputTokens = 0;

            public override async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
            {
                ChatResponse response=null;
                try
                {
                    response = await base.GetResponseAsync(chatMessages, options, cancellationToken);

                    if (response.RawRepresentation is OpenAI.Chat.ChatCompletion openAICompletion)
                    {
                        TotalInputTokens += openAICompletion.Usage.InputTokenCount;
                        TotalOutputTokens += openAICompletion.Usage.OutputTokenCount;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Usage: InputTokenCount={openAICompletion.Usage.InputTokenCount}, OutputTokenCount={openAICompletion.Usage.OutputTokenCount}");
                        Console.WriteLine($"Usage: TotalInputToken={TotalInputTokens}, TotalOutputTokenCount={TotalOutputTokens}");
                        Console.ResetColor();
                    }
                }
                catch 
                {
                  
                }

                return response;
            }
        }
    }
}
