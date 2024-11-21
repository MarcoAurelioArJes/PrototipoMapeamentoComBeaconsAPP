using PrototipoMapeamentoAPP.Configuracao;
using System.Diagnostics;

namespace PrototipoMapeamentoAPP.Model
{
    public class Mapa
    {
        public No[,] Nos { get; }
        public static int LarguraMapa => (int)ConfiguracaoDoMapa.LarguraMapa / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz;
        public static int AlturaMapa  => (int)ConfiguracaoDoMapa.AlturaMapa / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz;
        public static int LarguraObstaculo => (int)ConfiguracaoDoMapa.LarguraObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz;
        public static int AlturaObstaculo => (int)ConfiguracaoDoMapa.AlturaObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz;
        public static int PosicaoXObstaculo => (int)ConfiguracaoDoMapa.PosicaoXObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz;
        public static int PosicaoYObstaculo => (int)ConfiguracaoDoMapa.PosicaoYObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; 

        public Mapa()
        {
            Nos = new No[LarguraMapa, AlturaMapa];
            for (int x = 0; x < LarguraMapa; x++)
            {
                for (int y = 0; y < AlturaMapa; y++)
                {
                    bool Andavel = !EstaDentroDoObstaculo(x, y);
                    Nos[x, y] = new No(x, y, Andavel);
                }
            }
        }

        public bool EstaDentroDoObstaculo(int x, int y)
        {
            return x >= PosicaoXObstaculo &&
                   x < PosicaoXObstaculo + LarguraObstaculo &&
                   y >= PosicaoYObstaculo &&
                   y < PosicaoYObstaculo + AlturaObstaculo;
        }

        public bool ValidarPosicao(int x, int y)
        {
            return  x >= 0 && 
                    y >= 0 &&
                    x < LarguraMapa && 
                    y < AlturaMapa;
        }

        public void LimparMapa()
        {
            for (int x = 0; x < LarguraMapa; x++)
            {
                for (int y = 0; y < AlturaMapa; y++)
                {
                    var no = Nos[x, y];
                    no.CustoCaminhoAtual = double.PositiveInfinity;
                    no.CustoEstimadoAteDestino = 0;
                    no.Pai = null;
                }
            }
        }

        public (double X, double Y) ObterDestinoXY(double posicaoX, double posicaoY)
        {
            if (Math.Round(posicaoX, MidpointRounding.AwayFromZero) == LarguraMapa)
                posicaoX = posicaoX - 1;
            if (Math.Round(posicaoY, MidpointRounding.AwayFromZero) == AlturaMapa)
                posicaoY = posicaoY - 1;

            if (posicaoX > LarguraMapa || posicaoY > AlturaMapa)
                throw new Exception("Não é possível alcançar o destino informado");

            return (posicaoX, posicaoY);
        }
    }
}
