using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonClasses
{
    public class ActionCommand:ICommand
    {
        private readonly Action<Object> action;
        private readonly Predicate<Object> predicate;
        #region Constructors
        public ActionCommand(Action<Object> action):this(action,null)
        {
            
        }

        public ActionCommand(Action<Object> action, Predicate<Object> predicate)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action","You have to specify action");
            }
            this.action = action;
            this.predicate = predicate;

        }
        #endregion
        #region ICommand implementation
        public bool CanExecute(object parameter)
        {
            if (predicate == null)
            {
                return true;
            }
            return predicate(parameter);

        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion
    }
}
