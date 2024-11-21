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
        private static Dictionary<string, (double MediaRSSIPadrao, int ADVInterval)> ConfiguracaoTXPowerEm4 => new Dictionary<string, (double MediaRSSIPadrao, int ADVInterval)>
        {
            { UUID_BEACON_A, new (-50, 500) },
            { UUID_BEACON_B, new (-58, 500) },
            { UUID_BEACON_C, new(-50, 500) },
            { UUID_BEACON_D, new(-58.5, 500) },
            { UUID_BEACON_E, new(-49, 500) }
        };

        //TXPOWER 6
        private static Dictionary<string, (double MediaRSSIPadrao, int ADVInterval)> ConfiguracaoTXPowerEm6 => new Dictionary<string, (double MediaRSSIPadrao, int ADVInterval)>
        {
            { UUID_BEACON_A, new (-44.5, 500) },
            { UUID_BEACON_B, new (-55.5, 500) },
            { UUID_BEACON_C, new (-45, 500) },
            { UUID_BEACON_D, new (-52, 500) },
            { UUID_BEACON_E, new (-45, 500) }
        };
        public static (double MediaRSSIPadrao, int ADVInterval) ObterConfiguracaoDoBeaconPorUUID(string uuid)
        {

            if (!ConfiguracaoTXPowerEm6.TryGetValue(uuid, out var beacon))
                throw new Exception($"Não existe UUID {uuid}");

            return (beacon.MediaRSSIPadrao, beacon.ADVInterval);
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