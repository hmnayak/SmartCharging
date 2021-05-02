using System;

namespace SmartChargingApi.Models.Api
{
    public class SuggestedConnector
    {
        public int ConnectorId { get; set; }

        public float MaxCurrentInAmps { get; set; }

        public string ChargeStationName { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SuggestedConnector other = (SuggestedConnector)obj;
                return ConnectorId == other.ConnectorId;
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(ConnectorId, MaxCurrentInAmps).GetHashCode();
        }
    }
}
