using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTest1.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IHubContext<MessageHub, IClientSystemUpdate> _hub;

        public TestController(IHubContext<MessageHub, IClientSystemUpdate> hub)
        {
            _hub = hub;
        }

        public IActionResult Index()
        {
            // if you have index.cshtml open then _hub should show 1 connection

            return Ok("Hello world");
        }
    }
}