using System;
using System.Timers;
using BTApplication.Models;
using CoreBluetooth;

namespace BTApplication.iOS
{
	public class BluetoothManager : CBCentralManagerDelegate
	{
		public override void UpdatedState(CBCentralManager central)
		{
			if (central.State == CBCentralManagerState.PoweredOn)
			{
				CBUUID[] cbuuids = null;
				central.ScanForPeripherals(cbuuids);

				var timer = new Timer(30 * 1000);

				timer.Elapsed += (sender, e) => central.StopScan();
			}
			else 
			{
				System.Console.WriteLine("Bluetooth is not available");
			}
		}

		public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSDictionary advertisementData, Foundation.NSNumber RSSI)
		{
			Console.WriteLine("Discovered {0}, data {1}, RSII {2}", peripheral.Name, advertisementData, RSSI);
		}
	}
}
