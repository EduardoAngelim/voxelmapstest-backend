using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VoxelMapsTestTask.Hub
{
    public class ProgressHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IHubContext<ProgressHub> _hubContext;

        public ProgressHub(IHubContext<ProgressHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task UpdateProgress(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            await Clients.Client(connectionId).SendAsync("ConnectionId", connectionId);
            await base.OnConnectedAsync();
        }
    }
}
