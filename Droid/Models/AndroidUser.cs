using Android.Bluetooth;
using BTApplication.Models;

namespace BTApplication.Droid.Models
{
    public class AndroidUser : User
    {
        public BluetoothDevice BluetoothDevice { get; set; }
    }
}