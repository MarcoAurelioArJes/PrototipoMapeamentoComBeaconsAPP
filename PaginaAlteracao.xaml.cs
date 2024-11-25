namespace PrototipoMapeamentoAPP;
using PrototipoMapeamentoAPP.Repository;
using PrototipoMapeamentoAPP.Model;
using System.Collections.ObjectModel;

using PrototipoMapeamentoAPP.Configuracao;

public partial class PaginaAlteracao : ContentPage
{
    private readonly PontoDeInteresseRepository _pontoDeInteresseRepository;
    public ObservableCollection<PontoDeInteresse> PontosDeInteresse { get; set; } = new();
    private readonly Mapa _mapa;

    public PaginaAlteracao(Mapa mapa)
    {
        InitializeComponent();
        _pontoDeInteresseRepository = new PontoDeInteresseRepository();
        BindingContext = this;
        _mapa = new Mapa();
        ListarPontosDeInteresse();
    }

    private async void ListarPontosDeInteresse()
    {
        var pontos = await _pontoDeInteresseRepository.ObterPorFiltro(string.Empty);
        PontosDeInteresse.Clear();
        foreach (var ponto in pontos)
        {
            PontosDeInteresse.Add(ponto);
        }
    }

    private async void SalvarAlteracoes(object sender, EventArgs e)
    {
        foreach (var ponto in PontosDeInteresse)
        {
            var destino = _mapa.ObterDestinoXY(ponto.PosicaoRealX, ponto.PosicaoRealY);
            ponto.PosicaoRealX = destino.X;
            ponto.PosicaoRealY = destino.Y;
            await _pontoDeInteresseRepository.Atualizar(ponto);
        }

        await DisplayAlert("Sucesso", "Alterações salvas com sucesso!", "OK");
        ListarPontosDeInteresse();
    }

    private async void ExcluirPonto(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is PontoDeInteresse ponto)
        {
            var confirmacao = await DisplayAlert("Confirmação", $"Deseja excluir o ponto '{ponto.Nome}'?", "Sim", "Não");
            if (confirmacao)
            {
                await _pontoDeInteresseRepository.Excluir(ponto);
                PontosDeInteresse.Remove(ponto);
                await DisplayAlert("Sucesso", "Ponto excluído com sucesso!", "OK");
            }
        }
    }
    private async void VoltarTelaInicial(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


}