using System;
using System.Diagnostics.Tracing;
using System.Windows.Input;

namespace ImageApp.Directory.ViewModels
{
    class RelayCommand : ICommand
    {
        private Action _action;
        private bool _execute = true;
        public bool CanExecute(object parameter)
        {
            return _execute;
        }

        public void Execute(object parameter)
        {
            switch (parameter)
            {
                case null:
                    _action();
                    return;
                case true:
                    _execute = true;
                    RaiseCanExecuteChanged();
                    return;
                case false:
                    _execute = false;
                    RaiseCanExecuteChanged();
                    break;
            }
        }

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, new EventArgs());
        }
    }
}
