using SQLite;

namespace PrototipoMapeamentoAPP.Model
{
    public class PontoDeInteresse
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public double PosicaoRealX { get; set; }
        public double PosicaoRealY { get; set; }
    }
}