using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class Chat
    {
        public int AccountTypeOfSender { get; set; }
        public int reqClientId { get; set; }
        public int SenderId { get; set; }
        public string? Receiver { get; set; }

        public List<ChatHistory> chatHistories { get; set; }
    }

    public class ChatHistory
    {
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Message { get; set; }
        public bool isMyMsg { get; set; }
        public TimeOnly CreatedAt { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}
