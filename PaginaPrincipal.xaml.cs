using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Services;

namespace PrototipoMapeamentoAPP;
public partial class PaginaPrincipal : ContentPage
{
    private readonly BeaconService _beaconService;

    public PaginaPrincipal()
    {
        InitializeComponent();

        canvasView.Drawable = new MapaDrawable();

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

        canvasView.Invalidate();
    }
}