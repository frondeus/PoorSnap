using System;
using System.Collections.Generic;
using System.Linq;
using BTApplication.Models;
using CoreBluetooth;
using CoreFoundation;
using System.Threading.Tasks;
using BTApplication.iOS.Models;

namespace BTApplication.iOS
{
	class CentralManager : CBCentralManagerDelegate
	{
		public List<CBPeripheral> DiscoveredPeripherals = new List<CBPeripheral>();

		public override void UpdatedState(CBCentralManager central)
		{
			Console.WriteLine("-- Update State: {0}", central.State);
		}

		public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSDictionary advertisementData, Foundation.NSNumber RSSI)
		{
			if (DiscoveredPeripherals.Any(p => p.Identifier == peripheral.Identifier) == false)
			{
				DiscoveredPeripherals.Add(peripheral);
			}
		}

		public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
		{
			Console.WriteLine("-- Connected {0}", peripheral.Name);
		}

	}
	public class BluetoothManager : IBluetoothManager
	{
		private CentralManager del;
		private CBCentralManager mgr;

		public IMessageHandler MessageHandler { get; set; }
		public IConnectionHandler ConnectionHandler { get; set; }

		public BluetoothManager()
		{
			Console.WriteLine("Create Bluetooth Manager for iOS");
			del = new CentralManager();
			mgr = new CBCentralManager(del, DispatchQueue.CurrentQueue);
		}

		public void SendMessage(Message message)
		{
			throw new NotImplementedException();
		}

		public async void Scan()
		{
			var foundDevices = await PerformScanAsync();
			ConnectionHandler.OnAvailableConnections(foundDevices);
		}

		private async Task<List<iOsUser>> PerformScanAsync()
		{
			CBUUID[] cbuuids = null;
			mgr.ScanForPeripherals(cbuuids);

			await Task.Delay(5000);

			mgr.StopScan();

			return del.DiscoveredPeripherals.Select(d => new iOsUser
			{
				Peripheral = d,
				Name = string.IsNullOrWhiteSpace(d.Name) ? "Undefined device" : d.Name
			}).ToList();
		}

		public void Connect(User user)
		{
			iOsUser usr = user as iOsUser;
			mgr.ConnectPeripheral(usr.Peripheral);
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}

	}
}
