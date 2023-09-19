using System.IO;
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

        
        public void UpdateSourceItem(FileItem item, string path)
        {
            var index = Source.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            
            item = Source[index];
            
            item.Path = path;
            item.Name = Path.GetFileNameWithoutExtension(path);
            item.FullName = Path.GetFileName(path);
            item.Extension = Path.GetExtension(path);
        }
    }
}