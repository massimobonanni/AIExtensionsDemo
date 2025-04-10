using Microsoft.Extensions.AI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace AIExtensionsDemo.Middleware
{
    internal static class UseLanguageStep
    {
        public static ChatClientBuilder UseLanguage(this ChatClientBuilder builder, string language)
            => builder.Use(inner => new UseLanguageChatClient(inner, language));

        private class UseLanguageChatClient(IChatClient inner, string language): DelegatingChatClient(inner)
        {
            public override async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
            {
                var languageInstructionMessage = new ChatMessage(ChatRole.User, $"Always reply in the language {language} whatever language the user chooses.");
                var chatMessages= new List<ChatMessage>(messages);
                chatMessages.Add(languageInstructionMessage);
                try
                {
                    return await base.GetResponseAsync(chatMessages, options, cancellationToken);
                }
                finally
                {
                }
            }

        }
    }
}
