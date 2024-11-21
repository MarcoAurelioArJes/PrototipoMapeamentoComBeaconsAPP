using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP
{
    public class MapaDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
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


            foreach (var beacon in ConfiguracaoBeacon.BeaconsConhecidos)
            {
                canvas.FillColor = Colors.Green;
                canvas.FillCircle((float)beacon.PosicaoNoMapa.X, (float)beacon.PosicaoNoMapa.Y, 10);
            }

            if (ConfiguracaoDoMapa.DestinoX > 0 && ConfiguracaoDoMapa.DestinoY > 0)
            {
                canvas.FillColor = Colors.Yellow;
                canvas.FillCircle(ConfiguracaoDoMapa.DestinoX * ConfiguracaoDoMapa.DivisorPixelParaMatriz, 
                                  ConfiguracaoDoMapa.DestinoY * ConfiguracaoDoMapa.DivisorPixelParaMatriz, 7);
            }

            if (ConfiguracaoDoMapa.Caminho.Count > 0)
            {
                canvas.StrokeColor = Colors.Blue;
                canvas.StrokeSize = 2;
                var meio = ConfiguracaoDoMapa.DivisorPixelParaMatriz / 2;
                for (int i = 0; i < ConfiguracaoDoMapa.Caminho.Count - 1; i++)
                {
                    canvas.DrawLine(ConfiguracaoDoMapa.Caminho[i].X + meio,
                    ConfiguracaoDoMapa.Caminho[i].Y + meio,
                    ConfiguracaoDoMapa.Caminho[i + 1].X + meio,
                    ConfiguracaoDoMapa.Caminho[i + 1].Y + meio);
                }
            }
        }
    }
}