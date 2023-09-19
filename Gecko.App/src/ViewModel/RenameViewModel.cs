using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.ViewModel
{
    public partial class RenameViewModel: ObservableObject
    {
        [ObservableProperty]
        private string _pattern = string.Empty;
        
        [ObservableProperty]
        private string _replace = string.Empty;
    }
}