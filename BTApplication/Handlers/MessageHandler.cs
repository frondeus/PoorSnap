using Xamarin.Forms;

namespace BTApplication.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        public void OnMessage(Models.Message message)
        {
            //tutaj trzeba dodac labela do viewgrupy
            //https://developer.xamarin.com/api/type/Android.Views.ViewGroup/
            //https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically
        }

        private Label CreateMessage(Models.Message message)
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