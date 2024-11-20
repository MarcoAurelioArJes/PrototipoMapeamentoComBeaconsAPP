using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP.Model
{
    public class Beacon
    {
        public string BeaconTAG { get; set; }
        public string UUID { get; set; }
        public double Distancia => Math.Pow(10, (ObterMediaRSSIs() - RSSI) / (10 * _fatorDePerdaDeSinal));
        public int RSSI { get; set; }
        public int ADVInterval => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).ADVInterval;
        
        private List<int> _rssis = new List<int>();
        private double _fatorDePerdaDeSinal = 2.0d;

        public void AdicionarRSSI(int rssi)
        {
            _rssis.Add(rssi);
            if (_rssis.Count > 0) 
                _rssis.RemoveRange(0, 59);
        }

        private double ObterMediaRSSIs() => _rssis.Average();
    }
}