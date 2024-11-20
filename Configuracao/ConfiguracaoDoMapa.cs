namespace PrototipoMapeamentoAPP.Configuracao
{
    public static class ConfiguracaoDoMapa
    {
        public static float PosicaoDoUsuarioX { get; set; } = 0f;
        public static float PosicaoDoUsuarioY { get; set; } = 0f;

        public static float LarguraMapa { get; set; } = 400f;
        public static float AlturaMapa { get; set; } = 800f;

        public static float LarguraObstaculo => LarguraMapa / 2;
        public static float AlturaObstaculo => AlturaMapa / 2;
        public static float PosicaoXObstaculo => LarguraObstaculo / 2;
        public static float PosicaoYObstaculo => AlturaObstaculo / 2;
    }
}