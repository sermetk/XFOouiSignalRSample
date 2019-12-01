using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OouiSignalRSample.Core.Base
{
    public class CustomCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public int ClickCount { get; set; }
        private bool IsClickLoading;
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;
        public CustomCommand(Action<object> execute, bool isClickLoading = true)
        {
            IsClickLoading = isClickLoading;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
        public CustomCommand(Action execute, bool isClickLoading = true) : this(o => execute())
        {
            IsClickLoading = isClickLoading;
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public CustomCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public CustomCommand(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);

            return true;
        }

        public void Execute(object parameter)
        {
            if (ClickCount > 0)
                return;
            ClickCount++;
            _execute(parameter);
            Task.Delay(600).ContinueWith((e) =>
            {
                ClickCount = 0;
            });
        }
    }
}