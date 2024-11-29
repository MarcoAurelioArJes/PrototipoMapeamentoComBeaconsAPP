using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP.Model
{
    public class Beacon
    {
        public string BeaconTAG { get; set; }
        public string UUID { get; set; }
        public Point PosicaoNoMapa => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).PosicaoNoMapa;
        public (double X, double Y) PosicaoReal =>
            (ConfiguracaoDoMapa.ConverterPixelsParaMetrosX(PosicaoNoMapa.X),
            ConfiguracaoDoMapa.ConverterPixelsParaMetrosY(PosicaoNoMapa.Y));
        public List<CalibracaoData> DadosCalibracao { get; set; } = new List<CalibracaoData>();
        public double Distancia => Math.Pow(10, (_rssi0calibrado - ObterMediaRSSIsAtuais()) / (10 * _expoentePerdaDeCaminho));
        public int RSSIAtual { get; set; }
        public double MediaRSSIPadrao => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).MediaRSSIPadrao;
        public int ADVInterval => ConfiguracaoBeacon.ObterConfiguracaoDoBeaconPorUUID(UUID).ADVInterval;
        
        private List<double> _rssis = new List<double>();
        private double _rssi0calibrado { get; set; } = -55.5d;
        private double _expoentePerdaDeCaminho { get; set; } = 2.0d;

        public void AdicionarRSSIAtual(double rssi)
        {
            _rssis.Add(rssi);
            if (_rssis.Count > 60)
                _rssis.RemoveRange(0, 59);
        }

        public double ObterMediaRSSIsAtuais() => _rssis.Count > 0 ? _rssis.Average() : RSSIAtual;

        public void AdicionarPontoDeCalibracao(double distancia, double rssi)
        {
            DadosCalibracao.Add(new CalibracaoData(distancia, rssi));
        }

        public void Calibrar()
        {
            if (DadosCalibracao.Count >= 2)
            {
                var logDistancias = DadosCalibracao.Select(cd => Math.Log10(cd.Distance)).ToArray();
                var rssis = DadosCalibracao.Select(cd => cd.RSSI).ToArray();

                double sumLogD = logDistancias.Sum();
                double sumRSSI = rssis.Sum();
                double sumLogD2 = logDistancias.Select(ld => ld * ld).Sum();
                double sumLogD_RSSI = logDistancias.Zip(rssis, (ld, rssi) => ld * rssi).Sum();

                int N = DadosCalibracao.Count;

                double slope = (N * sumLogD_RSSI - sumLogD * sumRSSI) / (N * sumLogD2 - sumLogD * sumLogD);
                double intercept = (sumRSSI - slope * sumLogD) / N;

                _rssi0calibrado = intercept;
                _expoentePerdaDeCaminho = -slope / 10.0;
            } else if (DadosCalibracao.Count == 0)
            {
                _rssi0calibrado = -55.5d;
                _expoentePerdaDeCaminho = 2.0d;
            }
        }
    }
}