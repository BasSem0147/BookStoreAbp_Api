using Microsoft.AspNetCore.SignalR;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Acme.BookStore.AppServices.SignalR
{
    public class NotificationRHub : Hub, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;

        public NotificationRHub(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public override Task OnConnectedAsync()
        {
            var userId = _currentUser.Id?.ToString();
            if (userId != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId); // 👤 Group by User ID
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = _currentUser.Id?.ToString();
            if (userId != null)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
