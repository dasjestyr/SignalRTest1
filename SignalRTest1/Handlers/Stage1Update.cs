using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;

namespace SignalRTest1.Handlers
{
    public class Stage1Update : IEvent
    {
        public string CallerId { get; set; }

        public string Message { get; set; }
    }

    public class Stage1UpdateHandler : IHandleMessages<Stage1Update>
    {
        private readonly IHubContext<MessageHub, IClientSystemUpdate> _hub;

        public Stage1UpdateHandler(IHubContext<MessageHub, IClientSystemUpdate> hub)
        {
            _hub = hub;
        }

        public async Task Handle(Stage1Update message, IMessageHandlerContext context)
        {
            // BUG: if you observe connections in _hub it says 0 instead of 1

            await _hub.Clients.All.PushUpdate(message.Message);
            Debug.WriteLine($"Pushed 2nd message to client {message.CallerId} (from Handler)!");
        }
    }
}
