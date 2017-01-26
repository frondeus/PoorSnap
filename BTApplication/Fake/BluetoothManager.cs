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
		private User currentUser;

		public IConnectionHandler ConnectionHandler { get; set; }

		public IMessageHandler MessageHandler { get; set; }

		async public void Connect(User user)
		{
			await Task.Delay(500);
			timer = new Timer(10000);
			var i = 0;
			timer.Elapsed += (sender, e) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Console.WriteLine(string.Format("Username: {0}", currentUser.Name));

                    var fakeMessage = new Message
                    {
                        TextContent = string.Format("Fake text message {0}", i),
                        Name = user.Name,
                        BgColor = "Green"

                    };

					i++;
					MessageHandler.OnMessage(fakeMessage);
				});
			};
			timer.Start();

			currentUser = user;
			ConnectionHandler.OnConnected(user);
		}

		async public void Disconnect()
		{
			if (timer == null)
			{
				return;
			}

			timer.Stop();

			Console.WriteLine("Disconnecting...");
			await Task.Delay(500);

			timer = null;
			ConnectionHandler.OnDisconnected();
		}

		public async void Scan()
		{
			await Task.Delay(500);

			var list = new List<User>();
			for (int i = 0; i < 20; i++)
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

			MessageHandler.OnMessage(message);

			await Task.Delay(1000);


			var fakeMessage = new Message
			{
				TextContent = string.Format("Echo message: {0}", message.TextContent),
				Name = currentUser.Name,
                BgColor = "Green"

			};

			MessageHandler.OnMessage(fakeMessage);
		}
	}
}
