using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eniverse.ServerModel
{
    [Table(nameof(MerchantProduct))]
    public class MerchantProduct
    {
        [Key, Column(Order = 0)]
        public int MerchantID { get; set; }

        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        public short Volume { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(MerchantID))]
        public Merchant Merchant { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
    }
}
