using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eniverse.ServerModel
{
    [Table(nameof(Merchant))]
    public class Merchant
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Credits { get; set; }

        [MaxLength(20)]
        [Required]
        public string StarshipName { get; set; }

        public short CargoHoldVolume { get; set; }

        public int CurrentStationID { get; set; }

        [ForeignKey(nameof(CurrentStationID))]
        public Station Station { get; set; }

        public ICollection<MerchantProduct> MerchantProducts { get; set; }
    }
}
