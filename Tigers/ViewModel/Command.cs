using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tigers.ViewModel
{
    abstract class BaseCommand : ICommand
    {
        protected Func<bool> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null) { return true; }
            else return this.canExecute();
        }

        public virtual void Execute(object parameter = null) { }

    }

    class Command : BaseCommand
    {
        private Action action;

        public Command(Action a, Func<bool> canExecute)
        {
            if (a == null) throw new ArgumentNullException("Akce nemá žádnou obsluhu");
            this.action = a;
            this.canExecute = canExecute;
        }

        public Command(Action a) : this(a, null) { }


        override public void Execute(object parameter = null)
        {
            this.action();
        }
    }

    class ParametrizedCommand : BaseCommand
    {
        private Action<object> action;

        public ParametrizedCommand(Action<object> action, Func<bool> canExecute)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Akce nemá žádnou obsluhu");
            }
            this.action = action;
            this.canExecute = canExecute;
        }
        public ParametrizedCommand(Action<object> action) : this(action, null) { }

        override public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
