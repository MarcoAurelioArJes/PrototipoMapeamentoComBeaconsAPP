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

        foreach (var beacon in ConfiguracaoBeacon.BeaconsConhecidos)
        {
            ConfiguracaoDoMapa.PosicaoDoUsuarioX = float.Parse(PosicaoX.Text);
            ConfiguracaoDoMapa.PosicaoDoUsuarioY = float.Parse(PosicaoY.Text);
        }

        //_mapa.ExibirMapa();
        _mapa.LimparMapa();
        var posicaoDoUsuario = _mapa.Nos[(int)ConfiguracaoDoMapa.PosicaoDoUsuarioX, (int)ConfiguracaoDoMapa.PosicaoDoUsuarioY];
        var destino = _mapa.Nos[8, 8];

        if (!_mapa.EstaDentroDoObstaculo(posicaoDoUsuario.X, posicaoDoUsuario.Y) && !_mapa.EstaDentroDoObstaculo(destino.X, destino.Y) &&
            _mapa.ValidarPosicao(posicaoDoUsuario.X, posicaoDoUsuario.Y) && _mapa.ValidarPosicao(destino.X, destino.Y))
        {
            var caminho = aEstrelaService.EncontrarCaminho(posicaoDoUsuario, destino);
            //var caminho = aEstrelaService.EncontrarCaminho2(posicaoDoUsuario, destino);

            if (caminho != null)
            {
                for (int i = 0; i < caminho.Count; i++)
                {
                    var no = caminho[i];
                    Trace.WriteLine($"{no.X}, {no.Y}");
                }
            }
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