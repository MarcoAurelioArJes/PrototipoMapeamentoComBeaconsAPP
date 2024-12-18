using System.Diagnostics;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Repository;
using System.Collections.ObjectModel;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    
    private readonly Mapa _mapa;
    private readonly AEstrelaService _aEstrelaService;
    private readonly BeaconService _beaconService;
    private readonly PontoDeInteresseRepository _pontoDeInteresseRepository;
    private Task _iniciarEscaneamento;

    public ObservableCollection<PontoDeInteresse> PontosFiltrados { get; set; } = new();

    public PaginaPrincipal()
    {
        InitializeComponent();
        
        canvasView.Drawable = new MapaDrawable();

        _mapa = new Mapa();
        _aEstrelaService = new AEstrelaService(_mapa);
        _beaconService = new BeaconService();
        _pontoDeInteresseRepository = new PontoDeInteresseRepository();
        PontosFiltrados = new ObservableCollection<PontoDeInteresse>();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //IniciarPontosDeInteresse();
        _iniciarEscaneamento = IniciarScan();
    }

    private void IniciarPontosDeInteresse()
    {
        var pontosDeInteresseAsync = _pontoDeInteresseRepository.ObterPorFiltro(string.Empty);
        pontosDeInteresseAsync.Wait();

        if (pontosDeInteresseAsync.Result.Count == 0)
        {
            _pontoDeInteresseRepository.Seed();
            var pontosDeInteresseSeedAsync = _pontoDeInteresseRepository.ObterPorFiltro(string.Empty);
            pontosDeInteresseSeedAsync.Wait();
            ConfiguracaoDoMapa.PontosDeInteresse = pontosDeInteresseSeedAsync.Result;
            return;
        }

        ConfiguracaoDoMapa.PontosDeInteresse = pontosDeInteresseAsync.Result;
    }

    private async Task IniciarScan()
    {
        while (true)
        {
            await _beaconService.AtualizarInformacoesDosBeacons();

            AtualizarPosicaoDoUsuario();

            await Task.Delay(800);
        }
    }

    private void AtualizarPosicaoDoUsuario()
   {
        var beaconsProximos = ConfiguracaoBeacon.BeaconsConhecidos.OrderBy(c => c.Distancia).ToList();
        var posicaoUsuarioCalculado = AlgoritmoDeTrilateracao.Trilaterar(beaconsProximos);

        ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoUsuarioCalculado.Value.X;
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoUsuarioCalculado.Value.Y;

        foreach (var beacondevice in beaconsProximos)
        {   
            Debug.WriteLine($"BeaconTAG: {beacondevice.BeaconTAG} RSSI: {beacondevice.RSSIAtual} DISTANCIA: {beacondevice.Distancia}");
            beacondevice.PrintarCalculoRSSI();
        }

        Debug.WriteLine($"Posi��o Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");

        //A*
        if (ConfiguracaoDoMapa.DestinoX != 0 || ConfiguracaoDoMapa.DestinoY != 0)
            EncontrarMelhorCaminho();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            canvasView.Invalidate();
        });
    }

    private void EncontrarMelhorCaminho()
    {
        _mapa.LimparMapa();
        var posicaoDoUsuarioAEstrela = _mapa.Nos[(int)(ConfiguracaoDoMapa.PosicaoDoUsuarioX / ConfiguracaoDoMapa.DivisorPixelParaMatriz),
                                                (int)(ConfiguracaoDoMapa.PosicaoDoUsuarioY / ConfiguracaoDoMapa.DivisorPixelParaMatriz)];

        var destino = _mapa.Nos[(int)ConfiguracaoDoMapa.DestinoX, (int)ConfiguracaoDoMapa.DestinoY];

        if (!_mapa.EstaDentroDoObstaculo(posicaoDoUsuarioAEstrela.X, posicaoDoUsuarioAEstrela.Y) && !_mapa.EstaDentroDoObstaculo(destino.X, destino.Y) &&
            _mapa.ValidarPosicao(posicaoDoUsuarioAEstrela.X, posicaoDoUsuarioAEstrela.Y) && _mapa.ValidarPosicao(destino.X, destino.Y))
        {
            var caminho = _aEstrelaService.EncontrarCaminho2(posicaoDoUsuarioAEstrela, destino);

            if (caminho != null)
            {
                ConfiguracaoDoMapa.Caminho = caminho.Select(c => (c.X, c.Y)).ToList();
                //ConfiguracaoDoMapa.PosicaoDoUsuarioX = caminho[1].X;
                //ConfiguracaoDoMapa.PosicaoDoUsuarioY = caminho[1].Y;
            }
        }
    }

    private void AoDigitarNaBarraDePesquisa(object sender, TextChangedEventArgs e)
    {
        string textoBusca = e.NewTextValue?.ToLower() ?? string.Empty;
        PontosFiltrados.Clear();
        foreach (var ponto in ConfiguracaoDoMapa.PontosDeInteresse.Where(p => p.Nome.ToLower().Contains(textoBusca)))
        {
            PontosFiltrados.Add(ponto);
        }
    }

    private void LimparPesquisa(object sender, EventArgs e)
    {
        searchBar.Text = string.Empty;

        if (ConfiguracaoDoMapa.Caminho != null)
            ConfiguracaoDoMapa.Caminho.Clear();

        if (PontosFiltrados != null && PontosFiltrados.Count > 0)
                PontosFiltrados.Clear();
    }


    private void AoSelecionarPontoDeInteresse(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is PontoDeInteresse pontoSelecionado)
        {
            var pontoDeInteresse = ConfiguracaoDoMapa.PontosDeInteresse.FirstOrDefault(c => c.Id == pontoSelecionado.Id);
            
            if (pontoDeInteresse != null)
            {
                var posicaoX = ConfiguracaoDoMapa.ConverterMetrosParaPixelsX(pontoDeInteresse.PosicaoRealX) / ConfiguracaoDoMapa.DivisorPixelParaMatriz;
                var posicaoY = ConfiguracaoDoMapa.ConverterMetrosParaPixelsY(pontoDeInteresse.PosicaoRealY) / ConfiguracaoDoMapa.DivisorPixelParaMatriz;

                var destino = _mapa.ObterDestinoXY(posicaoX, posicaoY);
                ConfiguracaoDoMapa.DestinoX = (float)destino.X;
                ConfiguracaoDoMapa.DestinoY = (float)destino.Y;

                ((ListView)sender).SelectedItem = null;

                PontosFiltrados.Clear();
                return;
            }

            DisplayAlert("Ponto de Interesse", $"Ponto de interesse n�o encontrado", "OK");
        }
    }

    private async void AbrirTelaDeCadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaCadastro(_mapa));
    }
    
    private async void AbrirTelaDeAlteracao(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaAlteracao(_mapa));
    }

    private async void AbrirTelaDeCalibracao(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaCalibracao());
    }
}