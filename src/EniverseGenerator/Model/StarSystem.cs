using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EniverseGenerator.Model
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

        public ICollection<Planet> Planets { get; set; }
    }
}
