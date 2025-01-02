using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.Middleware
{
    internal static class UseLanguageStep
    {
        public static ChatClientBuilder UseLanguage(this ChatClientBuilder builder, string language)
            => builder.Use(inner => new UseLanguageChatClient(inner, language));

        private class UseLanguageChatClient(IChatClient inner, string language): DelegatingChatClient(inner)
        {
            public override async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
            {
                var languageInstructionMessage= new ChatMessage(ChatRole.User, $"Always reply in the language {language} whatever language the user chooses.");
                chatMessages.Add(languageInstructionMessage);

                try
                {
                    return await base.CompleteAsync(chatMessages, options, cancellationToken);
                }
                finally
                {
                    chatMessages.Remove(languageInstructionMessage);
                }
            }
        }
    }
}
