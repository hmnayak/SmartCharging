using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartChargingApi.Models.Domain
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GroupId { get; private set; }

        public string Name { get; set; }

        public float CapacityInAmps { get; set; }

        [Required]
        public virtual ICollection<ChargeStation> ChargeStations { get; set;  }

        public Group()
        {
        }

        public Group(string name, float capacityInAmps, IEnumerable<ChargeStation> chargeStations)
        {
            Name = name;
            CapacityInAmps = capacityInAmps;
            ChargeStations = chargeStations.ToList();
        }
    }
}
