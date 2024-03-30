using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domian
{
    public class Currency : BaseEntity
    {

        public string Name { get; set; }

        public string Sign { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ExchageHistory> ExchageHistory { get; set; }
    }
}
