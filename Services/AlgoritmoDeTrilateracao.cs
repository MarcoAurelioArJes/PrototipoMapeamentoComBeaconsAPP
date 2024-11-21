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

            double coordenadaXDesconhecida = (constanteC * coeficienteE - constanteF * coeficienteB)
                                             / (coeficienteE * coeficienteA - coeficienteB * coeficienteD);

            double coordenadaYDesconhecida = (constanteC * coeficienteD - coeficienteA * constanteF)
                                             / (coeficienteB * coeficienteD - coeficienteA * coeficienteE);

            return new Point(ConfiguracaoDoMapa.ConverterMetrosParaPixelsX(coordenadaXDesconhecida),
                             ConfiguracaoDoMapa.ConverterMetrosParaPixelsY(coordenadaYDesconhecida));
        }
    }
}