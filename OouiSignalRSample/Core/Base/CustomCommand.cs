using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OouiSignalRSample.Core.Base
{
    public class CustomCommand : ICommand
    {
        public int ClickCount { get; set; }
        public event EventHandler CanExecuteChanged;
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public CustomCommand(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public CustomCommand(Action execute) : this(o => execute())
        {
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