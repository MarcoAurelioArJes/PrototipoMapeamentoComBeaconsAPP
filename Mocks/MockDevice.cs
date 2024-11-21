using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using PrototipoMapeamentoAPP.Configuracao;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrototipoMapeamentoAPP.Mocks
{
    public class MockDevice : IDevice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rssi { get; set; }
        public object NativeDevice => true;
        public DeviceState State => DeviceState.Disconnected;
        public IReadOnlyList<AdvertisementRecord> AdvertisementRecords => throw new NotImplementedException();
        public bool IsConnectable => true;
        public bool SupportsIsConnectable => true;
        public DeviceBondState BondState => DeviceBondState.Bonded;

        private List<int> rssiList;

        public MockDevice(string UUID, List<int> rssiList)
        {
            Id = Guid.Parse(UUID);
            this.rssiList = rssiList;
        }

        public int ReadRssiAsync(int count)
        {
            var rssi = rssiList[count];
            return rssi;
        }

        
        public Task<bool> ConnectAsync(bool autoConnect = false)
        {
            
            return Task.FromResult(true);
        }

        public Task DisconnectAsync()
        {
            
            return Task.CompletedTask;
        }

        public Task<IService> GetServiceAsync(Guid uuid)
        {
            
            return Task.FromResult<IService>(null);
        }

        public Task<IService> GetServiceAsync(string uuid)
        {
            
            return Task.FromResult<IService>(null);
        }

        public Task<IReadOnlyList<IService>> GetServicesAsync(CancellationToken cancellationToken = default)
        {
            
            return Task.FromResult<IReadOnlyList<IService>>(new List<IService>());
        }

        public Task<IService> GetServiceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            return Task.FromResult<IService>(null);
        }

        public Task<bool> UpdateRssiAsync()
        {
            return Task.FromResult(true);
        }

        public Task<int> RequestMtuAsync(int requestValue)
        {
            return Task.FromResult(23);
        }

        public bool UpdateConnectionInterval(ConnectionInterval interval)
        {
            return true;
        }

        public bool UpdateConnectionParameters(ConnectParameters connectParameters = default)
        {
            return true;
        }

        public void Dispose()
        {
        }

        public static List<MockDevice> GetMockDevices()
        {
            var beaconA = new MockDevice(ConfiguracaoBeacon.UUID_BEACON_A, new List<int> { -80, -90, -85, -90 });
            var beaconB = new MockDevice(ConfiguracaoBeacon.UUID_BEACON_B, new List<int> { -80, -90, -85, -90 });
            var beaconC = new MockDevice(ConfiguracaoBeacon.UUID_BEACON_C, new List<int> { -70, -72, -65, -60 });
            var beaconD = new MockDevice(ConfiguracaoBeacon.UUID_BEACON_D, new List<int> { -50, -55, -60, -65 });
            var beaconE = new MockDevice(ConfiguracaoBeacon.UUID_BEACON_E, new List<int> { -45, -47, -49, -50 });

            return new List<MockDevice> { beaconA, beaconB, beaconC, beaconD, beaconE };
        }
    }
}
