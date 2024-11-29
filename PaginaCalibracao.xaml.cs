using PrototipoMapeamentoAPP.Configuracao;
using PrototipoMapeamentoAPP.Model;
using PrototipoMapeamentoAPP.Services;
using System.Collections.ObjectModel;

namespace PrototipoMapeamentoAPP;

public partial class PaginaCalibracao : ContentPage
{
    private Beacon beaconSelecionado;
    private BeaconService _beaconService;
    private Task _atualizarRSSI;
    private ObservableCollection<CalibracaoData> _calibrationDataList = new ObservableCollection<CalibracaoData>();


    public PaginaCalibracao()
    {
        InitializeComponent();
        BeaconPicker.ItemsSource = ConfiguracaoBeacon.BeaconsConhecidos;
        _beaconService = new BeaconService();

        CalibrationDataList.ItemsSource = new List<string>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _atualizarRSSI = AtualizarRSSI();
    }

    private async Task AtualizarRSSI()
    {
        while (true)
        {
            await _beaconService.AtualizarInformacoesDosBeacons();
            await Task.Delay(2000);
        }
    }

    private void BeaconPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        beaconSelecionado = BeaconPicker.SelectedItem as Beacon;
        AtualizarListaDeCalibracao();
    }

    private void AtualizarListaDeCalibracao()
    {
        if (beaconSelecionado != null)
        {
            _calibrationDataList = new ObservableCollection<CalibracaoData>(beaconSelecionado.DadosCalibracao);
            CalibrationDataList.ItemsSource = _calibrationDataList;
        }
        else
        {
            CalibrationDataList.ItemsSource = null;
        }
    }

    private void AdicionarPontoDeCalibracao(object sender, EventArgs e)
    {
        if (beaconSelecionado == null)
        {
            DisplayAlert("Erro", "Selecione um Beacon", "OK");
            return;
        }

        if (double.TryParse(DistanceEntry.Text, out double distancia))
        {
            double rssi = beaconSelecionado.RSSIAtual;

            if (rssi == 0)
            {
                DisplayAlert("Erro", "RSSI n�o dispon�vel. Certifique-se de que o Beacon est� sendo detectado.", "OK");
                return;
            }

            var novoPonto = new CalibracaoData(distancia, rssi);
            beaconSelecionado.AdicionarPontoDeCalibracao(distancia, rssi);

            _calibrationDataList.Add(novoPonto);

            DistanceEntry.Text = string.Empty;
        }
        else
        {
            DisplayAlert("Erro", "Digite uma dist�ncia v�lida.", "OK");
        }
    }

    private async void CalibrationDataList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var item = e.Item as CalibracaoData;
        if (item != null)
        {
            bool confirmar = await DisplayAlert("Remover Ponto de Calibra��o", $"Deseja remover o ponto com Dist�ncia {item.Distance} m e RSSI {item.RSSI} dBm?", "Sim", "N�o");
            if (confirmar)
            {
                _calibrationDataList.Remove(item);

                beaconSelecionado.DadosCalibracao.Remove(item);
            }
        }

        ((ListView)sender).SelectedItem = null;
    }


    private void CalibrarBeacon(object sender, EventArgs e)
    {
        if (beaconSelecionado == null)
        {
            DisplayAlert("Erro", "Selecione um Beacon", "OK");
            return;
        }

        beaconSelecionado.Calibrar();
        DisplayAlert("Sucesso", $"Beacon {beaconSelecionado.BeaconTAG} calibrado com sucesso!", "OK");
    }

    private async void VoltarTelaPrincipal(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
