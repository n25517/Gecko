using System.Windows;
using Gecko.App.Model;
using Gecko.App.Repository;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private ICollectionView _files;
        private readonly ObservableCollection<FileItem> _source;

        public MainViewModel()
        {
            _source = Explorer.GetFileItemsOnPath(App.Args["--path"]);
            _files = CollectionViewSource.GetDefaultView(_source);
        }

        [RelayCommand]
        private void SelectAll()
        {
            foreach (FileItem file in Files)
            {
                file.IsSelected = true;
            }
        }
        
        [RelayCommand]
        private void SearchQuery(string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return;
            }
            
            try
            {
                Files.Filter = o => Regex.Match((o as FileItem)!.FullName!, search).Success;
                Files.Refresh();
            }
            catch (RegexParseException e)
            {
                MessageBox.Show("Regular expression error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        [RelayCommand]
        private void SearchTextChange(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return;
            }
            
            Files.Filter = null;
            Files.Refresh();
        }

        [RelayCommand]
        private void MoveSelectedFiles()
        {
            var items = Files.Cast<FileItem>().Where(file => file.IsSelected);
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.Contains(App.Args["--path"]))
                    {
                        MessageBox.Show("Please, select another path");
                    }
                    else
                    {
                        Parallel.ForEach(items, item =>
                        {
                            File.Move(Path.GetFullPath(item.Path),dialog.SelectedPath + Path.DirectorySeparatorChar + item.FullName, true);
                            Application.Current.Dispatcher.BeginInvoke(() => _source.Remove(item));
                        });
                    }
                }
            }
        }
        
        [RelayCommand]
        private void DeleteSelectedFiles()
        {
            var items = Files.Cast<FileItem>().Where(file => file.IsSelected);
            Parallel.ForEach(items, item =>
            {
                File.Delete(Path.GetFullPath(item.Path));
                Application.Current.Dispatcher.BeginInvoke(() => _source.Remove(item));
            });
            
            Files.Refresh();
        }
    }
}