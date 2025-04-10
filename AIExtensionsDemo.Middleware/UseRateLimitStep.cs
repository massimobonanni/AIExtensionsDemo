using Azure;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.RateLimiting;


namespace AIExtensionsDemo.Middleware
{
    internal static class UseRateLimitStep
    {
        public static ChatClientBuilder UseRateLimit(this ChatClientBuilder builder, TimeSpan window)
            => builder.Use(inner => new RateLimitedChatClient(inner,window));

        private class RateLimitedChatClient(IChatClient inner,TimeSpan window) : DelegatingChatClient(inner)
        {

            RateLimiter rateLimit = new FixedWindowRateLimiter(new()
            {
                Window = window,
                QueueLimit = 1,
                PermitLimit = 1,
            });

            public override async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
            {
                var lease= rateLimit.AttemptAcquire();
                if (!lease.IsAcquired)
                {
                    return new (new ChatMessage(ChatRole.Assistant, 
                        "SKIPPING DUE TO RATE LIMIT. TRY AGAIN LATER."));
                }

                return await base.GetResponseAsync(chatMessages, options, cancellationToken);
            }
        }
    }
}
