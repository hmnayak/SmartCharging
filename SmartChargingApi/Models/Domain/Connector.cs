using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartChargingApi.Models.Domain
{
    public class Connector
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ConnectorId { get; set; }

        public float MaxCurrentInAmps { get; set; }

        public int ChargeStationId { get; set; }
        public ChargeStation ChargeStation { get; set; } 

        public Connector()
        {

        }

        public Connector(float maxCurrentInAmps)
        {
            MaxCurrentInAmps = maxCurrentInAmps;
        }
    }
}
