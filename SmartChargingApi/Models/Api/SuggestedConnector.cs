namespace SmartChargingApi.Models.Api
{
    public class SuggestedConnector
    {
        public int ConnectorId { get; set; }

        public float MaxCurrentInAmps { get; set; }

        public string ChargeStationName { get; set; }
    }
}
