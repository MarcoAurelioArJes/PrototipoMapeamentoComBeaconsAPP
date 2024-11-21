using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using System.Diagnostics;
using PrototipoMapeamentoAPP.Configuracao;
using System;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    //private List<string> items;
    private Mapa _mapa;
    private AEstrelaService aEstrelaService;
    private readonly BeaconService _beaconService;
    private Task _iniciarEscaneamento;
    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

        _mapa = new Mapa();
        aEstrelaService = new AEstrelaService(_mapa);
        _beaconService = new BeaconService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        IniciarTeste();
    }

    private void IniciarTeste()
    {
        _iniciarEscaneamento = IniciarScan();
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

        //ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoUsuarioCalculado.Value.X;
        //ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoUsuarioCalculado.Value.Y;

        Random random = new Random();
        ConfiguracaoDoMapa.PosicaoDoUsuarioX = 0 + (float)random.NextDouble() * (200 - 0);
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = 0 + (float)random.NextDouble() * (400 - 0);

        Debug.WriteLine($"BeaconTAG: {beaconsProximos[0].BeaconTAG} RSSI: {beaconsProximos[0].RSSIAtual} DISTANCIA: {beaconsProximos[0].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[1].BeaconTAG} RSSI: {beaconsProximos[1].RSSIAtual} DISTANCIA: {beaconsProximos[1].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[2].BeaconTAG} RSSI: {beaconsProximos[2].RSSIAtual} DISTANCIA: {beaconsProximos[2].Distancia}");
        Debug.WriteLine($"Posição Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");
        Debug.WriteLine($"Posição Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");

        //A*
        _mapa.LimparMapa();
        var posicaoDoUsuarioAEstrela = _mapa.Nos[(int)(ConfiguracaoDoMapa.PosicaoDoUsuarioX / ConfiguracaoDoMapa.DivisorPixelParaMatriz),
                                                (int)(ConfiguracaoDoMapa.PosicaoDoUsuarioY / ConfiguracaoDoMapa.DivisorPixelParaMatriz)];
        //var posicaoDoUsuario = _mapa.Nos[2, 2];
        var destino = _mapa.Nos[(int)ConfiguracaoDoMapa.DestinoX , (int)ConfiguracaoDoMapa.DestinoY];

        if (!_mapa.EstaDentroDoObstaculo(posicaoDoUsuarioAEstrela.X, posicaoDoUsuarioAEstrela.Y) && !_mapa.EstaDentroDoObstaculo(destino.X, destino.Y) &&
            _mapa.ValidarPosicao(posicaoDoUsuarioAEstrela.X, posicaoDoUsuarioAEstrela.Y) && _mapa.ValidarPosicao(destino.X, destino.Y))
        {
            //var caminho = aEstrelaService.EncontrarCaminho(posicaoDoUsuarioAEstrela, destino);
            var caminho = aEstrelaService.EncontrarCaminho2(posicaoDoUsuarioAEstrela, destino);

            if (caminho != null)
            {
                ConfiguracaoDoMapa.Caminho = caminho.Select(c => (c.X, c.Y)).ToList();
            }
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            canvasView.Invalidate();
        });
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
}