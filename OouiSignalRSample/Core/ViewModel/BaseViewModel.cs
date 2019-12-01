using System;
using System.Linq;
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
        #endregion Properties

        #region Methods
        public abstract void Init();
        public virtual void InitCommands()
        {
        }
        #endregion

        #region Tasks
        public async Task LoadData(params Func<Task>[] execute)
        {
            try
            {
                IsClickLoading = true;
                var tasks = execute.Select(c => c());
                await Task.WhenAll(tasks);
                GC.Collect();
            }
            catch (Exception)
            {

            }
            finally
            {
                IsClickLoading = false;
            }

        }
        public async Task LoadInitData(params Func<Task>[] execute)
        {
            try
            {
                IsPageLoading = true;
                var tasks = execute.Select(c => c());
                await Task.WhenAll(tasks);
                GC.Collect();
            }
            catch (Exception)
            {

            }
            finally
            {
                IsPageLoading = false;
            }
        }
        #endregion
    }
}
