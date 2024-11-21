using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Configuracao
{
    public class ConfiguracaoBeacon
    {
        public const string UUID_BEACON_A = "00000000-0000-0000-0000-fbc9d511c66b";
        public const string UUID_BEACON_B = "00000000-0000-0000-0000-dc2d4fd10133";
        public const string UUID_BEACON_C = "00000000-0000-0000-0000-fa8621c7182d";
        public const string UUID_BEACON_D = "00000000-0000-0000-0000-e6d71436211a";
        public const string UUID_BEACON_E = "00000000-0000-0000-0000-fcf76150aae3";

        //TXPOWER 4
        private static Dictionary<string, (double MediaRSSIPadrao, int ADVInterval, Point PosicaoNoMapa)> ConfiguracaoTXPowerEm4 => new()
        {
            { UUID_BEACON_A, new (-50, 500, new Point(0, 0)) },
            { UUID_BEACON_B, new (-58, 500, new Point(ConfiguracaoDoMapa.LarguraMapa, 0)) },
            { UUID_BEACON_C, new(-50, 500, new Point(ConfiguracaoDoMapa.LarguraMapa / 2, ConfiguracaoDoMapa.AlturaMapa / 2)) },
            { UUID_BEACON_D, new(-58.5, 500, new Point(ConfiguracaoDoMapa.AlturaMapa, 0)) },
            { UUID_BEACON_E, new(-49, 500, new Point(ConfiguracaoDoMapa.LarguraMapa, ConfiguracaoDoMapa.AlturaMapa)) }
        };

        //TXPOWER 6
        private static Dictionary<string, (double MediaRSSIPadrao, int ADVInterval, Point PosicaoNoMapa)> ConfiguracaoTXPowerEm6 => new()
        {
            { UUID_BEACON_A, new (-44.5, 500, new Point(0, 0)) },
            { UUID_BEACON_B, new (-55.5, 500, new Point(ConfiguracaoDoMapa.LarguraMapa, 0)) },
            { UUID_BEACON_C, new (-45, 500, new Point(ConfiguracaoDoMapa.LarguraMapa / 2, ConfiguracaoDoMapa.AlturaMapa / 2)) },
            { UUID_BEACON_D, new (-52, 500, new Point(0, ConfiguracaoDoMapa.AlturaMapa)) },
            { UUID_BEACON_E, new (-45, 500, new Point(ConfiguracaoDoMapa.LarguraMapa, ConfiguracaoDoMapa.AlturaMapa)) }
        };
        public static (double MediaRSSIPadrao, int ADVInterval, Point PosicaoNoMapa) ObterConfiguracaoDoBeaconPorUUID(string uuid)
        {

            if (!ConfiguracaoTXPowerEm6.TryGetValue(uuid, out var beacon))
                throw new Exception($"Não existe UUID {uuid}");

            return (beacon.MediaRSSIPadrao, beacon.ADVInterval, beacon.PosicaoNoMapa);
        }

        public static List<Beacon> BeaconsConhecidos { get; set; } = new List<Beacon>()
        {
            new Beacon
            {
                BeaconTAG = nameof(UUID_BEACON_A),
                UUID = UUID_BEACON_A
            },
            new Beacon
            {
                BeaconTAG = nameof(UUID_BEACON_B),
                UUID = UUID_BEACON_B
            },
            new Beacon
            {
                BeaconTAG = nameof(UUID_BEACON_C),
                UUID = UUID_BEACON_C
            },
            new Beacon
            {
                BeaconTAG = nameof(UUID_BEACON_D),
                UUID = UUID_BEACON_D
            },
            new Beacon
            {
                BeaconTAG = nameof(UUID_BEACON_E),
                UUID = UUID_BEACON_E
            }
        };
    }
}