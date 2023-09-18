using System.Windows;
using Gecko.App.Model;
using Gecko.App.Repository;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private ICollectionView _files = Explorer.GetFileItemsOnPath(App.Args["--path"]);

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
    }
}