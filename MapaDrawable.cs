using PrototipoMapeamentoAPP.Configuracao;
﻿
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

        //public void DesenharNo(No no)
        //{
        //    // Desenhe um círculo em cada posição (X, Y) do nó no canvas
        //    // Isso pode ser ajustado de acordo com o tamanho do seu mapa e a escala do canvas
        //    var canvas = new Canvas();
        //    var paint = new Paint
        //    {
        //        Color = Colors.Blue,  // Cor do caminho (pode ser ajustada)
        //        Style = PaintStyle.Fill
        //    };

        //    // Exemplo de como desenhar o nó
        //    canvas.DrawCircle(no.X * 20, no.Y * 20, 10, paint);  // A escala pode ser ajustada conforme necessário
        //}
    }
}