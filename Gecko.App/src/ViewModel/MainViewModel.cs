using Gecko.App.Model;
using Gecko.App.Repository;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FileItem> _files;

        public MainViewModel()
        {
            const string _root = @"E:\Music"; // TODO: Test path
            _files = Explorer.GetFileItemsOnPath(_root, "*");
        }

        [RelayCommand]
        private void SelectAll()
        {
            foreach (var file in Files)
            {
                file.IsSelected = true;
            }
        }
    }
}