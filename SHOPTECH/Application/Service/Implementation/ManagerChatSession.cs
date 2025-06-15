using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service.Interface;
using GenerativeAI;

namespace Application.Service.Implementation
{
    public class ManagerChatSession: IManagerChatSession
    {

        private readonly Dictionary<string, ChatSession> _chatSessions;

        public ManagerChatSession()
        {
            _chatSessions = new Dictionary<string, ChatSession>();
        }

        public ChatSession GetChatId(string jwt)
        {
            ChatSession chatSession;
            if (!_chatSessions.TryGetValue(jwt, out chatSession))
            {
                return null;
            }
            return chatSession;
        }

        public void SetChatId(string jwt, ChatSession chatSession)
        {
            _chatSessions.Add(jwt, chatSession);
        }
    }
}
