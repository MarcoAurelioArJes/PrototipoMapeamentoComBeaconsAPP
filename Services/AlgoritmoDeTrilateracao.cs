using MathNet.Numerics;
using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Services
{
    public static class AlgoritmoDeTrilateracao
    {
        public static Point? Trilaterar(List<Beacon> beacons)
        {
            Beacon beacon1 = beacons[0];
            Beacon beacon2 = beacons[1];
            Beacon beacon3 = beacons[2];

            double coeficienteA = 2 * (beacon2.PosicaoReal.X - beacon1.PosicaoReal.X);

            double coeficienteB = 2 * (beacon2.PosicaoReal.Y - beacon1.PosicaoReal.Y);

            double constanteC = beacon1.Distancia * beacon1.Distancia - beacon2.Distancia * beacon2.Distancia
                                - beacon1.PosicaoReal.X * beacon1.PosicaoReal.X - beacon1.PosicaoReal.Y * beacon1.PosicaoReal.Y
                                + beacon2.PosicaoReal.X * beacon2.PosicaoReal.X + beacon2.PosicaoReal.Y * beacon2.PosicaoReal.Y;

            double coeficienteD = 2 * (beacon3.PosicaoReal.X - beacon2.PosicaoReal.X);

            double coeficienteE = 2 * (beacon3.PosicaoReal.Y - beacon2.PosicaoReal.Y);

            double constanteF = beacon2.Distancia * beacon2.Distancia - beacon3.Distancia * beacon3.Distancia
                                - beacon2.PosicaoReal.X * beacon2.PosicaoReal.X - beacon2.PosicaoReal.Y * beacon2.PosicaoReal.Y
                                + beacon3.PosicaoReal.X * beacon3.PosicaoReal.X + beacon3.PosicaoReal.Y * beacon3.PosicaoReal.Y;

            double dividendoXDesconhecida = (constanteC * coeficienteE - constanteF * coeficienteB);
            double divisorXDesconhecida = (coeficienteE * coeficienteA - coeficienteB * coeficienteD);

            double coordenadaXDesconhecida = dividendoXDesconhecida / (dividendoXDesconhecida.IsFinite() ? 1 : dividendoXDesconhecida);

            double dividendoYDesconhecida = (constanteC * coeficienteD - coeficienteA * constanteF);
            double divisorYDesconhecida = (coeficienteB * coeficienteD - coeficienteA * coeficienteE);

            double coordenadaYDesconhecida = dividendoYDesconhecida / (divisorYDesconhecida.IsFinite() ? 1 : divisorYDesconhecida);

            return new Point(ConfiguracaoDoMapa.ConverterMetrosParaPixelsX(Math.Abs(coordenadaXDesconhecida)),
                             ConfiguracaoDoMapa.ConverterMetrosParaPixelsY(Math.Abs(coordenadaYDesconhecida)));
        }
    }
}