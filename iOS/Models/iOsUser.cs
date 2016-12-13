using System;
using BTApplication.Models;
using CoreBluetooth;

namespace BTApplication.iOS.Models
{
	public class iOsUser : User
	{
		public CBPeripheral Peripheral { get; set; }
	}
}
