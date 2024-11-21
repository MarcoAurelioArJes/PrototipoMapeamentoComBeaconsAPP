namespace PrototipoMapeamentoAPP.Services
{
    public class AlgoritmoEstimacaoPorMaximaVerossimilhancaService
    {
        public static (double, double) EstimacaoPorMaximaVerossimilhanca(double[] distancias, (double x, double y)[] posicoesBeacons, double sigma = 1.0)
        {
            int numeroBeacons = posicoesBeacons.Length;

            if (numeroBeacons < 3)
                throw new ArgumentException("É necessário pelo menos 3 beacons para trilateração.");

            double posicaoUsuarioX = 0;
            double posicaoUsuarioY = 0;

            double taxaAprendizado = 0.01;
            int maximoIteracoes = 1000;
            double tolerancia = 1e-6;

            for (int iteracao = 0; iteracao < maximoIteracoes; iteracao++)
            {
                double gradienteX = 0;
                double gradienteY = 0;

                for (int i = 0; i < numeroBeacons; i++)
                {
                    double posicaoBeaconX = posicoesBeacons[i].x;
                    double posicaoBeaconY = posicoesBeacons[i].y;

                    double distanciaBeacon = distancias[i];

                    double distanciaEstimativa = Math.Sqrt((posicaoUsuarioX - posicaoBeaconX) * (posicaoUsuarioX - posicaoBeaconX) + (posicaoUsuarioY - posicaoBeaconY) * (posicaoUsuarioY - posicaoBeaconY));

                    gradienteX += -(distanciaBeacon - distanciaEstimativa) * (posicaoUsuarioX - posicaoBeaconX) / (distanciaEstimativa * sigma * sigma);
                    gradienteY += -(distanciaBeacon - distanciaEstimativa) * (posicaoUsuarioY - posicaoBeaconY) / (distanciaEstimativa * sigma * sigma);
                }

                posicaoUsuarioX -= taxaAprendizado * gradienteX;
                posicaoUsuarioY -= taxaAprendizado * gradienteY;

                if (Math.Sqrt(gradienteX * gradienteX + gradienteY * gradienteY) < tolerancia)
                {
                    break;
                }
            }

            return (posicaoUsuarioX, posicaoUsuarioY);
        }
    }
}