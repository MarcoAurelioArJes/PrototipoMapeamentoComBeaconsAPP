using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP.Model
{
    public class Beacon
    {
        public string BeaconTAG { get; set; }
        public string UUID { get; set; }
        public Point Position { get; set; }
        public double Distancia => Math.Pow(10, (MediaRSSIPadrao - ObterMediaRSSIsAtuais()) / (10 * _fatorDePerdaDeSinal));
        public int RSSIAtual { get; set; }
        public double MediaRSSIPadrao => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).MediaRSSIPadrao;
        public int ADVInterval => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).ADVInterval;
        
        private List<double> _rssis = new List<double>();
        private double _fatorDePerdaDeSinal = 2.0d;

        public void AdicionarRSSIAtual(double rssi)
        {
            _rssis.Add(rssi);
            if (_rssis.Count > 60)
                _rssis.RemoveRange(0, 59);
        }

        public double ObterMediaRSSIsAtuais() => _rssis.Average();
    }
}