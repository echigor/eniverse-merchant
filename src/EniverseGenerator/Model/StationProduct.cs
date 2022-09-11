using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EniverseGenerator.Model
{
    [Table(nameof(StationProduct))]
    public class StationProduct
    {
        [Key, Column(Order = 0)]
        public int StationID { get; set; }

        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        public short AvailableVolume { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(StationID))]
        public Station Station { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
    }
}
