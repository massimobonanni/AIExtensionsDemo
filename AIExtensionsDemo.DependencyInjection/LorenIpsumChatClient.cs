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

        public Task<ChatResponse> GetResponseAsync(IEnumerable<Microsoft.Extensions.AI.ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var message = new Microsoft.Extensions.AI.ChatMessage(ChatRole.Assistant, Faker.Lorem.Sentence());
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

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<Microsoft.Extensions.AI.ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var updates = Faker.Lorem.Words(Faker.RandomNumber.Next(10, 100));
            foreach (var update in updates)
            {
                yield return new ChatResponseUpdate(ChatRole.Assistant, update);
                await Task.Delay(Faker.RandomNumber.Next(10, 100), cancellationToken);
            }
        }
    }
}
