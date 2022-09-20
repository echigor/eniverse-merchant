using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eniverse.Model
{
    [Table(nameof(Planet))]
    public class Planet
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(13)]
        [Required]
        public string Name { get; set; }

        public int StarSystemID { get; set; }

        [ForeignKey(nameof(StarSystemID))]
        public StarSystem StarSystem { get; set; }

        public ICollection<Station> Stations { get; set; }
    }
}
