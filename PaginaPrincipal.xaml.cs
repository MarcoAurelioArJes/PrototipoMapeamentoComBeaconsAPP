using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using System.Diagnostics;
using PrototipoMapeamentoAPP.Configuracao;

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

        ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoDoUsuario1.X;
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoDoUsuario1.Y;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            canvasView.Invalidate();
        });

        //_mapa.ExibirMapa();

        var posicaoDoUsuario = new No((int)ConfiguracaoDoMapa.PosicaoDoUsuarioX, (int)ConfiguracaoDoMapa.PosicaoDoUsuarioY, true);
        var destino = new No(10, 10, true);
        var caminho = aEstrelaService.EncontrarCaminho(posicaoDoUsuario, destino);

        for (int i = 0; i < caminho.Count; i++)
        {
            var no = caminho[i];
            Trace.WriteLine(no.PodeAndar ? "C" : "O");
        }

        //if (caminho.Any())
        //{
        //    // Obter o contexto de desenho
        //    var drawable = canvasView.Drawable as MapaDrawable;

        //    // Itera sobre cada nó no caminho encontrado
        //    foreach (var no in caminho)
        //    {
        //        // Chama um método do MapaDrawable para desenhar o nó
        //        drawable?.DesenharNo(no);
        //    }
        //}
    }
}