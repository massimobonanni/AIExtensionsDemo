using Microsoft.Extensions.AI;
using OpenAI.Chat;
using System.Runtime.CompilerServices;

namespace AIExtensionsDemo.DependencyInjection
{
    internal class LorenIpsumChatClient : IChatClient
    {
        public ChatClientMetadata Metadata => throw new NotImplementedException();

        public void Dispose()
        {

        }

        public Task<ChatResponse> GetResponseAsync(IList<Microsoft.Extensions.AI.ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var message = new Microsoft.Extensions.AI.ChatMessage();
            message.Role = ChatRole.Assistant;

            message.Text = Faker.Lorem.Sentence();

            return Task.FromResult(new ChatResponse(message));
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            if (serviceType == null)
                throw new Exception();

            return
            serviceKey is not null ? null :
                serviceType == typeof(LorenIpsumChatClient) ? this :
                serviceType.IsInstanceOfType(this) ? this :
                null;
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<Microsoft.Extensions.AI.ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var updates = Faker.Lorem.Words(Faker.RandomNumber.Next(10, 100));
            foreach (var update in updates)
            {
                yield return new ChatResponseUpdate { Text = $"{update} " };
                await Task.Delay(Faker.RandomNumber.Next(10, 100), cancellationToken);
            }
        }
    }
}
