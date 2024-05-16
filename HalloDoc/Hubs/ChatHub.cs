using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.Data;
using Microsoft.AspNetCore.SignalR;
namespace HalloDoc.Hubs
{
    public class ChatHub : Hub
    {
       
        public async Task SendMessage(string user, string message,int AccountTypeOfSender,  int AccountTypeOfReceiver, int reqClientId)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, AccountTypeOfSender, AccountTypeOfReceiver, reqClientId);
        }
    }
}
