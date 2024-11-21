
using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Services
{
    public class AlgoritmoDosMinimosQuadradosPonderados
    {
        /// <summary>
        /// Estima a posição do usuário utilizando o Método dos Mínimos Quadrados Ponderados.
        /// </summary>
        /// <param name="beacons">Lista de beacons com posições conhecidas e distâncias medidas.</param>
        /// <returns>Ponto estimado da posição do usuário.</returns>
        public static Point EstimarPosicao(List<Beacon> beacons)
        {
            if (beacons == null || beacons.Count < 3)
                throw new ArgumentException("São necessários pelo menos 3 beacons para estimar a posição.");

            int quantidadeBeacons = beacons.Count;
            double[,] matrizEquacoes = new double[quantidadeBeacons - 1, 2];
            double[] resultadosEquacoes = new double[quantidadeBeacons - 1];
            double[] pesosBeacons = new double[quantidadeBeacons - 1];

            var beaconReferencia = beacons[0];
            double xReferencia = beaconReferencia.PosicaoNoMapa.X;
            double yReferencia = beaconReferencia.PosicaoNoMapa.Y;
            double distanciaReferencia = beaconReferencia.Distancia;

            for (int i = 1; i < quantidadeBeacons; i++)
            {
                var beaconAtual = beacons[i];
                double xBeacon = beaconAtual.PosicaoNoMapa.X;
                double yBeacon = beaconAtual.PosicaoNoMapa.Y;
                double distanciaBeacon = beaconAtual.Distancia;

                matrizEquacoes[i - 1, 0] = 2 * (xBeacon - xReferencia);
                matrizEquacoes[i - 1, 1] = 2 * (yBeacon - yReferencia);
                resultadosEquacoes[i - 1] = Math.Pow(distanciaReferencia, 2) - Math.Pow(distanciaBeacon, 2)
                                            - Math.Pow(xReferencia, 2) + Math.Pow(xBeacon, 2)
                                            - Math.Pow(yReferencia, 2) + Math.Pow(yBeacon, 2);

                pesosBeacons[i - 1] = 1 / distanciaBeacon;
            }

            var matrizTransposta = Transpor(matrizEquacoes);
            var matrizDiagonalPesos = CriarMatrizDiagonal(pesosBeacons);
            var produtoTranspostaPesos = Multiplicar(matrizTransposta, matrizDiagonalPesos);
            var produtoFinal = Multiplicar(produtoTranspostaPesos, matrizEquacoes);
            var resultadoFinal = Multiplicar(produtoTranspostaPesos, resultadosEquacoes);

            var posicaoEstimativa = ResolverSistemaLinear(produtoFinal, resultadoFinal);

            return new Point(posicaoEstimativa[0], posicaoEstimativa[1]);
        }

        private static double[,] Transpor(double[,] matriz)
        {
            int linhas = matriz.GetLength(0);
            int colunas = matriz.GetLength(1);
            var resultado = new double[colunas, linhas];

            for (int i = 0; i < linhas; i++)
                for (int j = 0; j < colunas; j++)
                    resultado[j, i] = matriz[i, j];

            return resultado;
        }

        private static double[,] Multiplicar(double[,] matrizA, double[,] matrizB)
        {
            int linhasA = matrizA.GetLength(0);
            int colunasA = matrizA.GetLength(1);
            int linhasB = matrizB.GetLength(0);
            int colunasB = matrizB.GetLength(1);

            if (colunasA != linhasB)
                throw new ArgumentException("Número de colunas de A deve ser igual ao número de linhas de B.");

            var resultado = new double[linhasA, colunasB];

            for (int i = 0; i < linhasA; i++)
                for (int j = 0; j < colunasB; j++)
                    for (int k = 0; k < colunasA; k++)
                        resultado[i, j] += matrizA[i, k] * matrizB[k, j];

            return resultado;
        }

        private static double[] Multiplicar(double[,] matrizA, double[] vetorB)
        {
            int linhasA = matrizA.GetLength(0);
            int colunasA = matrizA.GetLength(1);

            if (colunasA != vetorB.Length)
                throw new ArgumentException("Número de colunas de A deve ser igual ao tamanho de B.");

            var resultado = new double[linhasA];

            for (int i = 0; i < linhasA; i++)
                for (int k = 0; k < colunasA; k++)
                    resultado[i] += matrizA[i, k] * vetorB[k];

            return resultado;
        }

        private static double[,] CriarMatrizDiagonal(double[] pesos)
        {
            int tamanho = pesos.Length;
            var resultado = new double[tamanho, tamanho];

            for (int i = 0; i < tamanho; i++)
                resultado[i, i] = pesos[i];

            return resultado;
        }

        private static double[] ResolverSistemaLinear(double[,] matrizA, double[] vetorB)
        {
            int tamanho = matrizA.GetLength(0);
            var resultado = new double[tamanho];

            var matriz = new MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(MathNet.Numerics.LinearAlgebra.Storage.DenseColumnMajorMatrixStorage<double>.OfArray(matrizA));
            var vetor = new MathNet.Numerics.LinearAlgebra.Double.DenseVector(vetorB);
            var solucao = matriz.Solve(vetor);

            return solucao.ToArray();
        }

    }
}