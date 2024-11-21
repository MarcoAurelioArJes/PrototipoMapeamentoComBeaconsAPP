using SQLite;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Repository.Seeds;

namespace PrototipoMapeamentoAPP.Repository
{
    public class PontoDeInteresseRepository
    {
        private readonly MapeamentoDatabase _mapeamentoDatabase;
        private readonly SQLiteAsyncConnection _database;
        public PontoDeInteresseRepository()
        {
            _mapeamentoDatabase = new MapeamentoDatabase();
            _database = _mapeamentoDatabase.Database;
        }

        public void Seed()
        {
            _database.InsertAllAsync(PontosDeInteresseSeed.PontosDeInteresses).Wait();
        }

        public void Criar(PontoDeInteresse pontoDeInteresse)
        {
            _database.InsertAsync(pontoDeInteresse);
        }

        public Task<List<PontoDeInteresse>> ObterPorFiltro(string texto)
        {
            texto = texto.Trim().ToLower();
            return _database.Table<PontoDeInteresse>()
                            .Where(c => texto == string.Empty || c.Nome.ToLower().Contains(texto))
                            .ToListAsync();
        }
        public async Task Atualizar(PontoDeInteresse pontoDeInteresse)
        {
            await _database.UpdateAsync(pontoDeInteresse);
        }

        public async Task Excluir(PontoDeInteresse pontoDeInteresse)
        {
            await _database.DeleteAsync(pontoDeInteresse);
        }
    }
}