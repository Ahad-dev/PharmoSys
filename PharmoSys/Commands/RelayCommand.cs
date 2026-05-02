using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PharmoSys.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _asyncExecute;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        // Sync constructor
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Async constructor
        public RelayCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute = null)
        {
            _asyncExecute = asyncExecute ?? throw new ArgumentNullException(nameof(asyncExecute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            if (_asyncExecute != null)
                await _asyncExecute(parameter);
            else
                _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

