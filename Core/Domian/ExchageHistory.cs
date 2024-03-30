using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domian
{
    public class ExchageHistory : BaseEntity
    {
        [ForeignKey("Currency")]
        public int CurId { get; set; }
        public DateTime ExchangeDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Rate { get; set; }

        public virtual Currency Currency { get; set; }

    }
}
