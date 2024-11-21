using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using System.Diagnostics;
using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Services;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    //private List<string> items;
    private Mapa _mapa;
    private AEstrelaService aEstrelaService;
    private readonly BeaconService _beaconService;

    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

        _mapa = new Mapa();
        aEstrelaService = new AEstrelaService(_mapa);
        _beaconService = new BeaconService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await IniciarScan();
    }

    private async Task IniciarScan()
    {
        while (true)
        {
            await _beaconService.AtualizarInformacoesDosBeacons();
        }
    }

    private void AoClicarEmAtualizarPosicaoDoUsuario(object sender, EventArgs e)
    {

        foreach(var beacon in ConfiguracaoBeacon.BeaconsConhecidos)
        {
            ConfiguracaoDoMapa.PosicaoDoUsuarioX = float.Parse(PosicaoX.Text);
            ConfiguracaoDoMapa.PosicaoDoUsuarioY = float.Parse(PosicaoY.Text);
        }

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

        canvasView.Invalidate();
    }
}