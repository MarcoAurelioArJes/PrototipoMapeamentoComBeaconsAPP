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

        _iniciarEscaneamento = IniciarScan();
    }

    private async Task IniciarScan()
    {
        while (true)
        {
            await _beaconService.AtualizarInformacoesDosBeacons();

            AtualizarPosicaoDoUsuario();
        }
    }

    private void AtualizarPosicaoDoUsuario()
    {
        var posicaoDoUsuario1 = AlgoritmoDosMinimosQuadradosPonderados.EstimarPosicao(ConfiguracaoBeacon.BeaconsConhecidos);

        //ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoDoUsuario1.X;
        //ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoDoUsuario1.Y;

        Random random = new Random();
        ConfiguracaoDoMapa.PosicaoDoUsuarioX = 0 + (float)random.NextDouble() * (200 - 0);
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = 0 + (float)random.NextDouble() * (400 - 0);

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
}