using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OouiSignalRSample.Modules.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientTestPage : ContentPage
    {
        public ClientTestPage()
        {
#if DEBUG
            BindingContext = new ClientTestPageViewModel();
            InitializeComponent();
#endif
        }
    }
}