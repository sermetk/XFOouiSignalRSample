using Autofac;
using OouiSignalRSample.IOC;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OouiSignalRSample.Modules.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
#if DEBUG            
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                BindingContext = scope.Resolve<TestPageViewModel>();
            }
            InitializeComponent();
#endif
        }
    }
}