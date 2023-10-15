using System.IO;
using System.Windows;
using Gecko.App.Model;
using Gecko.App.Modal;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Forms = System.Windows.Forms;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using ModernWpf.Controls;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private FileItems _files = new(App.Args["--path"]);
        
        [RelayCommand]
        private void SelectAll()
        {
            foreach (FileItem file in Files.Filtered)
            {
                if (file.IsSelected)
                {
                    continue;
                }
                
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
                Files.Filtered.Filter = o => Regex.Match((o as FileItem)!.FullName, search).Success;
                Files.Filtered.Refresh();
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
            
            Files.Filtered.Filter = null;
            Files.Filtered.Refresh();
        }
        
        
        [RelayCommand]
        private void MoveSelected()
        {
            using var dialog = new Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() != Forms.DialogResult.OK)
            {
                return;
            }
            
            Parallel.ForEach(Files.GetSelectedFiles(), item =>
            {
                var path = dialog.SelectedPath + Path.DirectorySeparatorChar + item.FullName;
                File.Move(Path.GetFullPath(item.Path),path, true);
            });
        }
        
        [RelayCommand]
        private void DeleteSelected()
        {
            Parallel.ForEach(Files.GetSelectedFiles(), item =>
            {
                File.Delete(Path.GetFullPath(item.Path));
            });
            
            Files.Filtered.Refresh();
        }

        [RelayCommand]
        private async Task RenameSelected()
        {
            var rename = new RenameDialog();
            if (rename.DataContext is RenameViewModel)
            {
                var result = await rename.ShowAsync();
                var context = rename.DataContext as RenameViewModel;

                if (result == ContentDialogResult.Primary)
                {
                    Parallel.ForEach(Files.GetSelectedFiles(), item =>
                    {
                        var name = Regex.Replace(item.FullName, context.Pattern, context.Replace);
                        var destFileName = Path.GetFullPath(item.Path).Replace(item.FullName, name.Trim());
                        
                        File.Move(Path.GetFullPath(item.Path), destFileName, true);
                    });
                }
            }
        }
    }
}