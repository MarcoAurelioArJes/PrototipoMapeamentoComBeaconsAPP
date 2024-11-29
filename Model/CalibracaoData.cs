namespace PrototipoMapeamentoAPP.Model
{
    public class CalibracaoData
    {
        public double Distance { get; set; }
        public double RSSI { get; set; }

        public CalibracaoData(double distance, double rssi)
        {
            Distance = distance;
            RSSI = rssi;
        }

        public override string ToString()
        {
            return $"Distância: {Distance} m, RSSI: {RSSI} dBm";
        }

        public override bool Equals(object obj)
        {
            if (obj is CalibracaoData other)
            {
                return Distance == other.Distance && RSSI == other.RSSI;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Distance.GetHashCode() ^ RSSI.GetHashCode();
        }
    }
}
