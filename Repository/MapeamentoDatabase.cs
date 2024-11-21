using SQLite;
using PrototipoMapeamentoAPP.Model;

namespace PrototipoMapeamentoAPP.Repository
{
    public class MapeamentoDatabase
    {
        public readonly SQLiteAsyncConnection Database;

        public MapeamentoDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mapeamento.db");
            Database = new SQLiteAsyncConnection(dbPath);

            Database.CreateTableAsync<PontoDeInteresse>().Wait();
        }
    }
}