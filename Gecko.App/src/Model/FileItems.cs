using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.Model
{
    public partial class FileItems: ObservableObject
    {
        [ObservableProperty]
        private ICollectionView _filtered;
        
        [ObservableProperty]
        private ObservableCollection<FileItem> _source;
        
        public FileItems(ObservableCollection<FileItem> source)
        {
            _source = source;
            _filtered = CollectionViewSource.GetDefaultView(_source);
        }

        public IEnumerable<FileItem> GetSelectedFiles()
        {
            return Filtered.Cast<FileItem>().Where(file => file.IsSelected);
        }
    }
}