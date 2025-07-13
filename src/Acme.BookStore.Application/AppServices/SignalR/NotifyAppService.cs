using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.AppServices.SignalR
{
    public class NotifyAppService:ApplicationService
    {
        private readonly IHubContext<NotificationRHub> _hubContext;

        public NotifyAppService(IHubContext<NotificationRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUserAsync(Guid userId, string message)
        {
            await _hubContext.Clients.Group(userId.ToString())
                .SendAsync("ReceiveNotification", message);
        }

        public async Task BroadcastAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
