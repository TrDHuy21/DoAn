using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerativeAI;

namespace Application.Service.Interface
{
    public interface IManagerChatSession
    {
        ChatSession GetChatId(string jwt);
        void SetChatId(string jwt, ChatSession chatSession);
    }
}
