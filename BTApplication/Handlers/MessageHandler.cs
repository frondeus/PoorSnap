using System.Threading.Tasks;
using Android.App;
using BTApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BTApplication.Handlers
{
    public class MessageHandler : Activity, IMessageHandler
    {
        public BTApplicationPage Page { get; set; }
        public void OnMessage(Message message)
        {
            RunOnUiThread(() =>
            {
                Page.result.Text += message.TextContent;
            });
            //tutaj trzeba dodac labela do viewgrupy
            //https://developer.xamarin.com/api/type/Android.Views.ViewGroup/
            //https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically
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