//using Plugin.BLE;
//using Plugin.BLE.Abstractions.Contracts;
//using Plugin.BLE.Abstractions.EventArgs;

//namespace PrototipoMapeamentoAPP.Services
//{
//    public class BluetoothLEService
//    {
//        private IBluetoothLE _bluetoothLE;
//        private IAdapter _adapter;

//        private int contarDispositivosConhecidosEncontrados = 0;

//        public BluetoothLEService() 
//        {
//            _bluetoothLE = CrossBluetoothLE.Current;
//            _adapter = _bluetoothLE.Adapter;

//            _adapter.DeviceDiscovered += AoIniciarEscaneamento;
//        }

//        private async void AoIniciarEscaneamento(object? sender, DeviceEventArgs e)
//        {
//            if (contarDispositivosConhecidosEncontrados >= 5)
//            {
//                contarDispositivosConhecidosEncontrados = 0;
//                await PararDeEscanearDispositivos();
//                return;
//            }

//            //var beacon = Beacon.OwnBeacons.FirstOrDefault(b => b.UUID == e.Device.Id.ToString());
//            //if (beacon != null)
//            //{
//            //    beacon.Rssi = e.Device.Rssi;
//            //    beacon.AddSelfBeaconInformation(beacon);
//            //    _devices.Add(e.Device);
//            //    contarDispositivosConhecidosEncontrados++;
//            //}
//        }

//        public async Task EscanearDispositivos()
//        {
//            if (_bluetoothLE.State == BluetoothState.On)
//            {
//                var status = await Permissions.RequestAsync<Permissions.Bluetooth>();
//                if (status == PermissionStatus.Granted)
//                {
//                    //if (_devices.Count > 0)
//                    //{
//                    //    foreach (var device in _devices)
//                    //    {
//                    //        await device.UpdateRssiAsync();
//                    //        var beacon = Beacon.OwnBeacons.FirstOrDefault(b => b.UUID == device.Id.ToString());
//                    //        beacon.Rssi = device.Rssi;
//                    //        beacon.AddSelfBeaconInformation(beacon);
//                    //        continue;
//                    //    }

//                    //    return;
//                    //}
//                    //await _adapter.StartScanningForDevicesAsync();

//                    //foreach (var device in _devices)
//                    //{
//                    //    await _adapter.ConnectToDeviceAsync(device);
//                    //}
//                }
//            }
//            else
//            {
//                throw new Exception("O Bluetooth está desligado.");
//            }
//        }


//        public async Task PararDeEscanearDispositivos()
//        {
//            await _adapter.StopScanningForDevicesAsync();
//        }
//    }
//}