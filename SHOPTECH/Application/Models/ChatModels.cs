using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    internal class ChatModels
    {
    }
    // Models/ChatModels.cs
    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
    public class ChatResponse
    {
        public string Message { get;set; }
        public string Response { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ConversationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class GeminiRequest
    {
        public List<Content> Contents { get; set; } = new List<Content>();
    }

    public class Content
    {
        public List<Part> Parts { get; set; } = new List<Part>();
    }

    public class Part
    {
        public string Text { get; set; }
    }

    public class GeminiResponse
    {
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        public Content Content { get; set; }
    }
}
