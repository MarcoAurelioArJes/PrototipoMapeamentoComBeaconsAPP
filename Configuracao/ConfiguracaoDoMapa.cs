﻿namespace PrototipoMapeamentoAPP.Configuracao
{
    public static class ConfiguracaoDoMapa
    {
        private static double LarguraTela => DeviceDisplay.MainDisplayInfo.Width <= 0 ? 400 : DeviceDisplay.MainDisplayInfo.Width;
        private static double AlturaTela => DeviceDisplay.MainDisplayInfo.Height <= 0 ? 800 : DeviceDisplay.MainDisplayInfo.Height;

        public static float PosicaoDoUsuarioX { get; set; } = 50f;
        public static float PosicaoDoUsuarioY { get; set; } = 50f;

        public static float LarguraMapa => LarguraTela <= 400 ? (float)LarguraTela : 200f;
        public static float AlturaMapa => AlturaTela <= 800 ? (float)AlturaTela : 400f;
        public static float DivisorPixelParaMatriz { get; set; } = 10;
        public static float LarguraObstaculo => LarguraMapa / 2;
        public static float AlturaObstaculo => AlturaMapa / 2;
        public static float PosicaoXObstaculo => LarguraObstaculo / 2;
        public static float PosicaoYObstaculo => AlturaObstaculo / 2;

        public static float LarguraReal = 25f;
        public static float AlturaReal = 30f;

        public static float ResolucaoHorizontal => LarguraReal / LarguraMapa;
        public static float ResolucaoVertical => AlturaReal / AlturaMapa;

        public static double ConverterPixelsParaMetrosX(double pixelsX)
        {
            return pixelsX * ResolucaoHorizontal;
        }

        public static double ConverterPixelsParaMetrosY(double pixelsY)
        {
            return pixelsY * ResolucaoVertical;
        }
    }
}