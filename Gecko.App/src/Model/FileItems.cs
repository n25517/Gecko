using System;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Gecko.App.Repository;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.Model
{
    public partial class FileItems: ObservableObject
    {
        private readonly string _path;
        
        [ObservableProperty]
        private ICollectionView _filtered;
        
        [ObservableProperty]
        private ObservableCollection<FileItem> _source;
        
        private readonly FileSystemWatcher _fileWatcher;

        public FileItems(string path)
        {
            _path = path;
            
            _source = Explorer.GetFileItemsOnPath(path);
            _filtered = CollectionViewSource.GetDefaultView(_source);
            
            _fileWatcher = new FileSystemWatcher(path);
            
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;
            
            _fileWatcher.Renamed += FileWatcherOnRenamed;
            _fileWatcher.Deleted += FileWatcherOnDeleted;
        }
        
        public IEnumerable<FileItem> GetSelectedFiles()
        {
            return Filtered.Cast<FileItem>().Where(file => file.IsSelected);
        }
        
        private void FileWatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!e.FullPath.Contains(_path))
                {
                    Source.Remove(Source.First(x => x.Path == e.OldFullPath));
                    return;
                }

                var item = Source.First(x => x.Path == e.OldFullPath);
            
                item.Path = e.FullPath;
                item.Name = Path.GetFileNameWithoutExtension(e.FullPath);
                item.FullName = Path.GetFileName(e.FullPath);
                item.Extension = Path.GetExtension(e.FullPath);

                item.IsSelected = false;
            });
        }

        private void FileWatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(() => Source.Remove(Source.First(x => x.Path == e.FullPath)));
        }
    }
}