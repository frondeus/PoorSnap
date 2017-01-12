using System;
using BTApplication.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Timers;
using Xamarin.Forms;

namespace BTApplication.Fake
{
	public class BluetoothManager : IBluetoothManager
	{
		private Timer timer = null;

		public IConnectionHandler ConnectionHandler { get; set; }

		public IMessageHandler MessageHandler { get; set; }

		public void Connect(User user)
		{
			timer = new Timer(5000);
			var i = 0;
			timer.Elapsed += (sender, e) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{

					var fakeMessage = new Message
					{
						TextContent = string.Format("Fake text message {0}", i)
					};

					i++;

					MessageHandler.OnMessage(fakeMessage);
				});
			};
			timer.Start();
            ConnectionHandler.OnConnected(user);
		}

		public void Disconnect()
		{
			if (timer == null)
			{
				return;
			}

			Console.WriteLine("Disconnecting...");

			timer.Stop();
			timer = null;
		}

		public async void Scan()
		{
			await Task.Delay(5000);

			var list = new List<User>();
			for (int i = 0; i < 3; i++)
			{
				var user = new User
				{
					Name = string.Format("Fake Device no. {0}", i)
				};

				list.Add(user);
			}

			ConnectionHandler.OnAvailableConnections(list);
		}

		public async void SendMessage(Message message)
		{
			await Task.Delay(1000);

			var fakeMessage = new Message
			{
				TextContent = string.Format("Echo message: {0}", message.TextContent)

			};

			MessageHandler.OnMessage(fakeMessage);
		}
	}
}
