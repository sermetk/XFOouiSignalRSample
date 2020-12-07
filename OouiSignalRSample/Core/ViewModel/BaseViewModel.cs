using System.Threading.Tasks;
using Xamarin.Forms;

namespace OouiSignalRSample.Core
{
    public abstract class BaseViewModel : BindableBase
    {
        #region Properties
        public NavigationPage RootPage
        {
            get
            {
                return Application.Current.MainPage as NavigationPage;
            }
        }

        private bool _isPageLoading;
        public bool IsPageLoading
        {
            get { return _isPageLoading; }
            set { _isPageLoading = value; OnPropertyChanged(); }
        }
        private bool _isClickLoading;

        public bool IsClickLoading
        {
            get { return _isClickLoading; }
            set { _isClickLoading = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        public abstract void Init();
        public virtual void InitCommands()
        {
        }
        #endregion

        #region Tasks
        public async Task LoadData(params Task[] tasks)
        {
            IsClickLoading = true;
            await Task.WhenAll(tasks);
            IsClickLoading = false;

        }
        public async Task LoadInitData(params Task[] tasks)
        {
            IsPageLoading = true;
            await Task.WhenAll(tasks);
            IsPageLoading = false;
        }
        #endregion
    }
}
