using Microsoft.Extensions.AI;

namespace AIExtensionsDemo.DependencyInjection
{
    internal class LorenIpsumChatClient : IChatClient
    {
        public ChatClientMetadata Metadata => throw new NotImplementedException();

        public Task<ChatCompletion> CompleteAsync(
            IList<ChatMessage> chatMessages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var message = new ChatMessage();
            message.Role = ChatRole.Assistant;

            message.Text = Faker.Lorem.Sentence();

            return Task.FromResult(new ChatCompletion(message));
        }

        public async IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(
            IList<ChatMessage> chatMessages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var updates = Faker.Lorem.Words(Faker.RandomNumber.Next(10,100));
            foreach (var update in updates)
            {
                yield return new StreamingChatCompletionUpdate { Text = $"{update} " };
                await Task.Delay(Faker.RandomNumber.Next(10, 100), cancellationToken); 
            }
        }

        public void Dispose()
        {

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
    }
}
