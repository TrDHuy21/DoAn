using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.EmailModel
{
    public enum RecipientType
    {
        To,     // Người nhận chính
        Cc,     // Người nhận copy
        Bcc     // Người nhận copy ẩn
    }
}
