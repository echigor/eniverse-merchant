using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eniverse.ServerModel
{
    [Table(nameof(Station))]
    public class Station
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(7)]
        [Required]
        public string Name { get; set; }

        public int PlanetID { get; set; }

        [ForeignKey(nameof(PlanetID))]
        public Planet Planet { get; set; }

        public ICollection<StationProduct> StationProducts { get; set; }

        public ICollection<Merchant> Merchants { get; set; }
    }
}
