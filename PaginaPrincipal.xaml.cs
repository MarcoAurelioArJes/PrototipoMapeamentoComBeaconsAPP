using PrototipoMapeamentoAPP.Services;
using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    private readonly BeaconService _beaconService;
    private Task _iniciarEscaneamento;
    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

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
        var posicaoDoUsuario = AlgoritmoDosMinimosQuadradosPonderados.EstimarPosicao(ConfiguracaoBeacon.BeaconsConhecidos);

        ConfiguracaoDoMapa.PosicaoDoUsuarioX = (float)posicaoDoUsuario.X;
        ConfiguracaoDoMapa.PosicaoDoUsuarioY = (float)posicaoDoUsuario.Y;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            canvasView.Invalidate();
        });
    }
}