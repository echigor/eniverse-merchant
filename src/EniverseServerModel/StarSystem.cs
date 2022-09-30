using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eniverse.ServerModel
{
    [Table(nameof(StarSystem))]
    public class StarSystem
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(19)]
        [Required]
        public string Name { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public double ZCoordinate { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal PlanetDuty { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal StationDuty { get; set; }

        public ICollection<Planet> Planets { get; set; }
    }
}
