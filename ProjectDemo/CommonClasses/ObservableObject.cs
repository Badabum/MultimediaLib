using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonClasses.Annotations;

namespace CommonClasses
{
    public class ObservableObject:INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when the value of property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises event for the property - propertyName
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
