namespace PrototipoMapeamentoAPP.Configuracao
{
    public static class ConfiguracaoDoMapa
    {
        private static double LarguraTela => DeviceDisplay.MainDisplayInfo.Width <= 0 ? 400 : DeviceDisplay.MainDisplayInfo.Width;
        private static double AlturaTela => DeviceDisplay.MainDisplayInfo.Height <= 0 ? 800 : DeviceDisplay.MainDisplayInfo.Height;

        public static float PosicaoDoUsuarioX { get; set; } = 0f;
        public static float PosicaoDoUsuarioY { get; set; } = 0f;

        public static float LarguraMapa => LarguraTela <= 400 ? (float)LarguraTela - 100: 400f;
        public static float AlturaMapa => AlturaTela <= 800 ? (float)AlturaTela - 100: 800f;

        public static float LarguraObstaculo => LarguraMapa / 2;
        public static float AlturaObstaculo => AlturaMapa / 2;
        public static float PosicaoXObstaculo => LarguraObstaculo / 2;
        public static float PosicaoYObstaculo => AlturaObstaculo / 2;

        public static float DivisorPixelParaMatriz { get; set; } = 10;
    }
}