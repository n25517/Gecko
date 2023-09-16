using System.Windows;
using Gecko.App.Model;
using System.Windows.Data;
using Gecko.App.Repository;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FileItem> _files;

        [ObservableProperty]
        private ICollectionView _filteredFiles;

        public MainViewModel()
        {
            _files = Explorer.GetFileItemsOnPath(App.Args["--path"], App.Args.TryGetValue("--pattern", out var value) ? value : "*");
            _filteredFiles = CollectionViewSource.GetDefaultView(Files);
        }

        [RelayCommand]
        private void SelectAll()
        {
            foreach (var file in FilteredFiles)
            {
                (file as FileItem).IsSelected = true;
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
                FilteredFiles.Filter = o => Regex.Match(((FileItem)o).FullName, search).Success;
                FilteredFiles.Refresh();
            }
            catch (RegexParseException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        [RelayCommand]
        private void SearchTextChange(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return;
            }
            
            FilteredFiles.Filter = null;
            FilteredFiles.Refresh();
        }
    }
}