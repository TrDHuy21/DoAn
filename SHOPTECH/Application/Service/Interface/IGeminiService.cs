﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Interface
{
    public interface IGeminiService
    {
        Task<string> SendMessageAsync(string message, string jwt);
    }
}
