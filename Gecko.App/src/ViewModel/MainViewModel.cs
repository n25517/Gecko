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
            _files = Explorer.GetFileItemsOnPath(App.Args["--path"], App.Args.TryGetValue("--pattern", out var value) ? value : "*");
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