using System.Threading.Tasks;
using BTApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace BTApplication.Handlers
{
	public class MessageHandler : IMessageHandler
	{
		public BTApplicationPage Page { get; set; }
		public void OnMessage(Message message)
		{
			Page.AddMessage(message.TextContent);
			Console.WriteLine(message.TextContent);
		}

		private Label CreateMessage(Message message)
		{
			return new Label()
			{
				Text = message.TextContent,
				TextColor = GetTextColor(message.isLocal),
			};
		}

		private Color GetTextColor(bool isLocal)
		{
			if (isLocal) return Color.Blue;
			return Color.Aqua;
		}
	}
}