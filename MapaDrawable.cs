using Microsoft.Maui.Graphics;
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


            foreach(var beacon in ConfiguracaoBeacon.BeaconsConhecidos)
            {
                canvas.FillColor = Colors.Green;
                canvas.FillCircle((float)beacon.PosicaoNoMapa.X, (float)beacon.PosicaoNoMapa.Y, 10);
            }
        }
    }
}