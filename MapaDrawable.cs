namespace PrototipoMapeamentoAPP
{
    public class MapaDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var larguraDaTela = DeviceDisplay.MainDisplayInfo.Width;
            var alturaDaTela = DeviceDisplay.MainDisplayInfo.Height;

            float larguraMapa = 400;
            float alturaMapa = 800;

            ConfiguracaoDoMapa.LarguraMapa = larguraDaTela <= 400 ? (float)larguraDaTela - 100 : larguraMapa;
            ConfiguracaoDoMapa.AlturaMapa = alturaDaTela <= 800 ? (float)alturaDaTela - 100 : alturaMapa;

            var retangulo = new RectF(0, 0, ConfiguracaoDoMapa.LarguraMapa, ConfiguracaoDoMapa.AlturaMapa);
            canvas.FillColor = Colors.DarkGray;
            canvas.FillRectangle(retangulo);


            var retanguloObstaculo = new RectF(ConfiguracaoDoMapa.PosicaoXObstaculo,
                                               ConfiguracaoDoMapa.PosicaoYObstaculo,
                                               ConfiguracaoDoMapa.LarguraObstaculo, 
                                               ConfiguracaoDoMapa.AlturaObstaculo);

            canvas.FillColor = Colors.Black;
            canvas.FillRectangle(retanguloObstaculo);

            canvas.FillColor = Colors.Red;
            canvas.FillCircle(ConfiguracaoDoMapa.PosicaoDoUsuarioX, ConfiguracaoDoMapa.PosicaoDoUsuarioY, 10);
        }
    }
}