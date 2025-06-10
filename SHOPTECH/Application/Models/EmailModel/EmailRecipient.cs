using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.EmailModel
{
    public class EmailRecipient
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public RecipientType Type { get; set; } = RecipientType.To;
    }
}
