using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using BTApplication.iOS.Models;
using BTApplication.Models;
using CoreBluetooth;
using CoreFoundation;

namespace BTApplication.iOS
{

	class PeripheralDelegate : CBPeripheralDelegate
	{
		public override void DiscoveredService(CBPeripheral peripheral, Foundation.NSError error)
		{
			Console.WriteLine("Discovered a service");
			foreach (var service in peripheral.Services)
			{
				Console.WriteLine(service.ToString());
				peripheral.DiscoverCharacteristics(service);
			}	
		}

		public override void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, Foundation.NSError error)
		{
			Console.WriteLine("Discovered characteristics of " + peripheral);
			foreach (var c in service.Characteristics)
			{
				Console.WriteLine(c.ToString());
				peripheral.ReadValue(c);
			}	
		}

		public override void UpdatedValue(CBPeripheral peripheral, CBDescriptor descriptor, Foundation.NSError error)
		{
			Console.WriteLine("Value of characteristic " + descriptor.Characteristic + " is " + descriptor.Value);	
		}

		public override void UpdatedCharacterteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, Foundation.NSError error)
		{
			Console.WriteLine("Value of characteristic " + characteristic.ToString() + " is " + characteristic.Value);
		}
	}

	class CentralManager : CBCentralManagerDelegate
	{
		private Timer timer;
		private List<CBPeripheral> discoveredPeripherals = new List<CBPeripheral>();

		private IConnectionHandler _connectionHandler = null;

		public CentralManager(IConnectionHandler connectionHandler)
		{
			_connectionHandler = connectionHandler;
		}

		public override void UpdatedState(CBCentralManager central)
		{
			Console.WriteLine("-- Update State: {0}", central.State);

			if (central.State == CBCentralManagerState.PoweredOn)
			{
				CBUUID[] cbuuids = null;
				central.ScanForPeripherals(cbuuids);

				timer = new Timer(5 * 1000);
				timer.Elapsed += (sender, e) =>
				{
					central.StopScan();

					var users = discoveredPeripherals.Select(p => new iOsUser
					{
						Name = p.Name,
						Guid = Guid.Parse(p.Identifier.ToString()),
						Peripheral = p
					});

					//TODO:
					//_connectionHandler.OnAvailableConnections(users);

					discoveredPeripherals.Clear();
					timer.Stop();
				};
				timer.Enabled = true;
			}
			else 
			{
				System.Console.WriteLine("Bluetooth is not available");
			}
		}

		public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSDictionary advertisementData, Foundation.NSNumber RSSI)
		{
			if (discoveredPeripherals.Any(p => p.Identifier == peripheral.Identifier) == false)
			{
				Console.WriteLine("-- Discovered {0}, data {1}, RSII {2}", peripheral.Name, advertisementData, RSSI);

				discoveredPeripherals.Add(peripheral);
			}
		}

		public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
		{
			Console.WriteLine("-- Connected {0}", peripheral.Name);

			if (peripheral.Delegate == null)
			{
				peripheral.Delegate = new PeripheralDelegate();
				peripheral.DiscoverServices();
			}
		}

	}
	public class BluetoothManager : IBluetoothManager
	{
		private CentralManager del;
		private CBCentralManager mgr;
		private List<User> discoveredUsers = new List<User>();

		public BluetoothManager()
		{
			del = new CentralManager(null); //TODO
			mgr = new CBCentralManager(del, DispatchQueue.CurrentQueue);
		}

		public void SendMessage(Message message)
		{
			throw new NotImplementedException();
		}

		public void Scan()
		{
			//TODO: Póki co skan wykonuje się automatycznie przy starcie aplikacji.
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
