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

        ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoUsuarioCalculado.Value.X;
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoUsuarioCalculado.Value.Y;

        Debug.WriteLine($"BeaconTAG: {beaconsProximos[0].BeaconTAG} RSSI: {beaconsProximos[0].RSSIAtual} DISTANCIA: {beaconsProximos[0].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[1].BeaconTAG} RSSI: {beaconsProximos[1].RSSIAtual} DISTANCIA: {beaconsProximos[1].Distancia}");
        Debug.WriteLine($"BeaconTAG: {beaconsProximos[2].BeaconTAG} RSSI: {beaconsProximos[2].RSSIAtual} DISTANCIA: {beaconsProximos[2].Distancia}");
        Debug.WriteLine($"Posi��o Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");
        Debug.WriteLine($"Posi��o Atualizada: X={ConfiguracaoDoMapa.PosicaoDoUsuarioX}, Y={ConfiguracaoDoMapa.PosicaoDoUsuarioY}");

        MainThread.BeginInvokeOnMainThread(() =>
        {
            canvasView.Invalidate();
        });

        //_mapa.ExibirMapa();
        //_mapa.LimparMapa();
        //var posicaoDoUsuario = _mapa.Nos[(int)ConfiguracaoDoMapa.PosicaoDoUsuarioX, (int)ConfiguracaoDoMapa.PosicaoDoUsuarioY];
        //var destino = _mapa.Nos[8, 8];

        //if (!_mapa.EstaDentroDoObstaculo(posicaoDoUsuario.X, posicaoDoUsuario.Y) && !_mapa.EstaDentroDoObstaculo(destino.X, destino.Y) &&
        //    _mapa.ValidarPosicao(posicaoDoUsuario.X, posicaoDoUsuario.Y) && _mapa.ValidarPosicao(destino.X, destino.Y))
        //{
        //    var caminho = aEstrelaService.EncontrarCaminho(posicaoDoUsuario, destino);
        //    //var caminho = aEstrelaService.EncontrarCaminho2(posicaoDoUsuario, destino);

        //    if (caminho != null)
        //    {
        //        for (int i = 0; i < caminho.Count; i++)
        //        {
        //            var no = caminho[i];
        //            Trace.WriteLine($"{no.X}, {no.Y}");
        //        }
        //    }
        //}


        //if (caminho.Any())
        //{
        //    // Obter o contexto de desenho
        //    var drawable = canvasView.Drawable as MapaDrawable;

        //    // Itera sobre cada n� no caminho encontrado
        //    foreach (var no in caminho)
        //    {
        //        // Chama um m�todo do MapaDrawable para desenhar o n�
        //        drawable?.DesenharNo(no);
        //    }
        //}
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