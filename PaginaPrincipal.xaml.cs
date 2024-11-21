using System.Diagnostics;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Repository;
using System.Collections.ObjectModel;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    //private List<string> items;
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

        IniciarPontosDeInteresse();
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

            await Task.Delay(1000);
        }
    }

    private void AtualizarPosicaoDoUsuario()
    {
        var beaconsProximos = ConfiguracaoBeacon.BeaconsConhecidos.OrderBy(c => c.Distancia).ToList();
        var posicaoUsuarioCalculado = Trilaterate(beaconsProximos[0], beaconsProximos[1], beaconsProximos[2]);

        ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoUsuarioCalculado.Value.X;
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoUsuarioCalculado.Value.Y;

        Debug.WriteLine($"BeaconTAG: {beaconsProximos[0].BeaconTAG} RSSI: {beaconsProximos[0].RSSIAtual} DISTANCIA: {beaconsProximos[0].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[1].BeaconTAG} RSSI: {beaconsProximos[1].RSSIAtual} DISTANCIA: {beaconsProximos[1].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[2].BeaconTAG} RSSI: {beaconsProximos[2].RSSIAtual} DISTANCIA: {beaconsProximos[2].Distancia}");
        Debug.WriteLine($"Posição Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");
        Debug.WriteLine($"Posição Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");

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

            DisplayAlert("Ponto de Interesse", $"Ponto de interesse não encontrado", "OK");
        }
    }
    public static Point? Trilaterate(Beacon b1, Beacon b2, Beacon b3)
    {
        double x1 = b1.PosicaoNoMapa.X, y1 = b1.PosicaoNoMapa.Y;
        double x2 = b2.PosicaoNoMapa.X, y2 = b2.PosicaoNoMapa.Y;
        double x3 = b3.PosicaoNoMapa.X, y3 = b3.PosicaoNoMapa.Y;

        double r1 = b1.Distancia;
        double r2 = b2.Distancia;
        double r3 = b3.Distancia;

        double A = 2 * (x2 - x1);
        double B = 2 * (y2 - y1);
        double C = r1 * r1 - r2 * r2 - x1 * x1 - y1 * y1 + x2 * x2 + y2 * y2;
        double D = 2 * (x3 - x2);
        double E = 2 * (y3 - y2);
        double F = r2 * r2 - r3 * r3 - x2 * x2 - y2 * y2 + x3 * x3 + y3 * y3;
        double x = (C * E - F * B) / (E * A - B * D);
        double y = (C * D - A * F) / (B * D - A * E);


        return new Point(x, y);
    }

    private async void AbrirTelaDeCadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaCadastro(_mapa));
    }
    
    private async void AbrirTelaDeAlteracao(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaAlteracao(_mapa)));
    }
}