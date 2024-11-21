using PrototipoMapeamentoAPP.Configuracao;
using System.Diagnostics;

namespace PrototipoMapeamentoAPP.Model
{
    public class Mapa
    {
        public No[,] Nos { get; }
        public static int LarguraMapa { get => (int)ConfiguracaoDoMapa.LarguraMapa / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }
        public static int AlturaMapa { get => (int)ConfiguracaoDoMapa.AlturaMapa / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }
        public static int LarguraObstaculo { get => (int)ConfiguracaoDoMapa.LarguraObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }
        public static int AlturaObstaculo { get => (int)ConfiguracaoDoMapa.AlturaObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }
        public static int PosicaoXObstaculo { get => (int)ConfiguracaoDoMapa.PosicaoXObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }
        public static int PosicaoYObstaculo { get => (int)ConfiguracaoDoMapa.PosicaoYObstaculo / (int)ConfiguracaoDoMapa.DivisorPixelParaMatriz; }

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

        public void ExibirMapa()
        {
            for (int y = 0; y < AlturaMapa; y++)
            {
                for (int x = 0; x < LarguraMapa; x++)
                {
                    Trace.WriteLine(Nos[x, y].PodeAndar ? "C " : "O ");
                }
                Trace.WriteLine("");
            }
        }
    }
}
