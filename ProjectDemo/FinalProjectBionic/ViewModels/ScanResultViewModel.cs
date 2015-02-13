using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinalProjectBionic.ViewModels
{
    public class ScanResultViewModel
    {
        public ScanResultViewModel(ICollection<string> foundedFiles)
        {
            FileName = new ObservableCollection<string>(foundedFiles);
            SelectedFiles = new ObservableCollection<string>();
        }
        public ICollection<string> FileName {get; private set;}
        public ICollection<string> SelectedFiles { get; set; } 
    }
}
