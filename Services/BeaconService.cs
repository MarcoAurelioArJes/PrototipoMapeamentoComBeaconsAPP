using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using PrototipoMapeamentoAPP.Configuracao;

namespace PrototipoMapeamentoAPP.Services
{
    public class BeaconService
    {
        private IBluetoothLE _bluetoothLE;
        private IAdapter _adapter;

        private int contarBeaconsEncontrados = 0;
        private List<IDevice> _beacons = new List<IDevice>();

        public BeaconService() 
        {
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = _bluetoothLE.Adapter;

            _adapter.DeviceDiscovered += AoIniciarEscaneamento;
        }

        private async void AoIniciarEscaneamento(object? sender, DeviceEventArgs e)
        {
            if (contarBeaconsEncontrados >= 5)
            {
                contarBeaconsEncontrados = 0;
                await PararDeEscanearDispositivos();
                return;
            }

            var beacon = ConfiguracaoBeacon.BeaconsConhecidos.FirstOrDefault(b => b.UUID == e.Device.Id.ToString());
            if (beacon != null)
            {
                beacon.RSSIAtual = e.Device.Rssi;
                beacon.AdicionarRSSIAtual(beacon.RSSIAtual);
                _beacons.Add(e.Device);

                contarBeaconsEncontrados++;
            }
        }

        public async Task AtualizarInformacoesDosBeacons()
        {
            if (_bluetoothLE.State == BluetoothState.On)
            {
                var status = await Permissions.RequestAsync<Permissions.Bluetooth>();
                if (status == PermissionStatus.Granted)
                {
                    if (_beacons.Count > 0)
                    {
                        foreach (var device in _beacons)
                        {
                            await device.UpdateRssiAsync();
                            
                            var beacon = ConfiguracaoBeacon.BeaconsConhecidos.FirstOrDefault(b => b.UUID == device.Id.ToString());
                            if (beacon != null)
                            {
                                beacon.RSSIAtual = device.Rssi;
                                beacon.AdicionarRSSIAtual(beacon.RSSIAtual);
                            }

                            continue;
                        }

                        return;
                    }

                    await _adapter.StartScanningForDevicesAsync();
                    foreach (var device in _beacons)
                    {
                        await _adapter.ConnectToDeviceAsync(device);
                    }
                }
            }
            else
            {
                throw new Exception("O Bluetooth está desligado.");
            }
        }

        public async Task PararDeEscanearDispositivos()
        {
            await _adapter.StopScanningForDevicesAsync();
        }
    }
}