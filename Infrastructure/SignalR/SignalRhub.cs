using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace fileshare.Infrastructure.SignalR
{
    public class SignalRhub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }
    }
}