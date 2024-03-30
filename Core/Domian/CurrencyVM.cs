using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domian
{
    public class CurrencyVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Sign { get; set; }

        [Required]
        [RegularExpression(@"^(\d*\.)?\d+$")]
        public decimal Rate { get; set; }
    }
}
