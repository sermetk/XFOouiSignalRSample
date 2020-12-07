using OouiSignalRSample.Modules.Support;
using System;

using Xamarin.Forms;

namespace OouiSignalRSample.Modules
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
#if DEBUG
            buttonTest.IsVisible = true;
#endif
        }

        private void OnEditorCompleted(object sender, EventArgs e)
        {
            var vm = BindingContext as ChatPageViewModel;
            vm.SendMessageCommand.Execute(null);
        }
    }
}
