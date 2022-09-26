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

        public int Credits { get; set; }

        [MaxLength(16)]
        [Required]
        public string StarshipName { get; set; }
       
        public int CurrentStationID { get; set; }

        [ForeignKey(nameof(CurrentStationID))]
        public Station Station { get; set; }

        public ICollection<MerchantProduct> MerchantProducts { get; set; }
    }
}
