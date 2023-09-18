using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.Model
{
    public partial class FileItem: ObservableObject
    {
        [ObservableProperty]
        private string _path;
        
        [ObservableProperty] 
        private string _name;

        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _extension;
        
        [ObservableProperty]
        private bool _isSelected = false;
    }
}