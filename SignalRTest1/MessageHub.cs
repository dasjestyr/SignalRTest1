using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using SignalRTest1.Handlers;

namespace SignalRTest1
{
    public interface IClientSystemUpdate
    {
        Task PushUpdate(object update);
    }

    public class MessageHub : Hub<IClientSystemUpdate>
    {
        private readonly IMessageSession _bus;
        private readonly IHubContext<MessageHub, IClientSystemUpdate> _hub;

        public MessageHub(IMessageSession bus, IHubContext<MessageHub, IClientSystemUpdate> hub)
        {
            _bus = bus;
            _hub = hub;
        }

        public async Task CauseUpdate()
        {
            // if you observe connections in _hub it should read 1

            await Clients.Caller.PushUpdate(new {Message = "Updating system in 2 seconds... "});
            Debug.WriteLine($"Pushed first message for client {Context.ConnectionId} (from Hub)!");

            await Task.Delay(2000);
            await _bus.Publish<Stage1Update>(ev => 
            {
                ev.CallerId = Context.ConnectionId;
                ev.Message = "Did the thing successfully!";
            });
            Debug.WriteLine("Fired event!");
        }
    }
}
