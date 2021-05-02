using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartChargingApi.Models.Domain
{
    public class ChargeStation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ChargeStationId { get; private set; }

        public string Name { get; set; }

        public ICollection<Connector> Connectors { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ChargeStation()
        {

        }

        public ChargeStation(string name, IEnumerable<Connector> connectors)
        {
            Name = name;
            Connectors = connectors.ToList();
        }
    }
}
