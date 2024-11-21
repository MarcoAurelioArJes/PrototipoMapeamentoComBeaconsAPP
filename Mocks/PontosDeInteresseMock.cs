using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Mocks
{
    public static class PontosDeInteresseMock
    {
        public static List<PontoDeInteresse> ObterPontosDeInteresse()
        {
            return new List<PontoDeInteresse>
            {
                new PontoDeInteresse { Nome = "Entrada Principal", PosicaoRealX = 2.5, PosicaoRealY = 3.0 },
                new PontoDeInteresse { Nome = "Recepção", PosicaoRealX = 5.0, PosicaoRealY = 7.5 },
                new PontoDeInteresse { Nome = "Auditório", PosicaoRealX = 15.0, PosicaoRealY = 10.0 },
                new PontoDeInteresse { Nome = "Cafeteria", PosicaoRealX = 10.0, PosicaoRealY = 25.0 },
                new PontoDeInteresse { Nome = "Saída de Emergência", PosicaoRealX = 22.0, PosicaoRealY = 28.0 }
            };
        }
    }
}