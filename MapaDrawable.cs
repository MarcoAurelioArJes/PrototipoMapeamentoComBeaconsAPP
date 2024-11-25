using Microsoft.Maui.Graphics.Text;
using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Mocks;

namespace PrototipoMapeamentoAPP
{
    public class MapaDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var retangulo = new RectF(0, 0, ConfiguracaoDoMapa.LarguraMapa, ConfiguracaoDoMapa.AlturaMapa);
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(retangulo);

            //var retanguloObstaculo = new RectF(ConfiguracaoDoMapa.PosicaoXObstaculo,
            //                                   ConfiguracaoDoMapa.PosicaoYObstaculo,
            //                                   ConfiguracaoDoMapa.LarguraObstaculo,
            //                                   ConfiguracaoDoMapa.AlturaObstaculo);

            //canvas.FillColor = Colors.LightGray;
            //canvas.FillRectangle(retanguloObstaculo);

            canvas.FillColor = Colors.Red;
            canvas.FillCircle(ConfiguracaoDoMapa.PosicaoDoUsuarioX, ConfiguracaoDoMapa.PosicaoDoUsuarioY, 10);


            if (ConfiguracaoDoMapa.PontosDeInteresse.Count > 0)
            {
                foreach (var pontoDeInteresse in ConfiguracaoDoMapa.PontosDeInteresse)
                {
                    canvas.FillColor = Colors.DeepPink;
                    float posicaoX = (float)ConfiguracaoDoMapa.ConverterMetrosParaPixelsX(pontoDeInteresse.PosicaoRealX);
                    float posicaoY = (float)ConfiguracaoDoMapa.ConverterMetrosParaPixelsY(pontoDeInteresse.PosicaoRealY);

                    HorizontalAlignment posicaoTexto = posicaoX == 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                    canvas.DrawString(pontoDeInteresse.Nome, posicaoX, posicaoY, posicaoTexto);
                    canvas.FillCircle(posicaoX, posicaoY, 10);
                }
            }


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