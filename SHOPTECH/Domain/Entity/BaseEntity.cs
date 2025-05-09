using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }

        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }

        //public int? CreatedBy { get; set; }

        //public int? UpdatedBy { get; set; }

        //public User? CreatedByUser { get; set; }
        //public User? UpdatedByUser { get; set; }


        public string? Description { get; set; }
        public bool IsActive { get; set; }

    }
}
