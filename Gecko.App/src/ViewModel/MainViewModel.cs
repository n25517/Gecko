﻿using System.IO;
using System.Windows;
using Gecko.App.Model;
using Gecko.App.Repository;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Forms = System.Windows.Forms;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gecko.App.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private FileItems _files = new(Explorer.GetFileItemsOnPath(App.Args["--path"]));
        
        [RelayCommand]
        private void SelectAll()
        {
            foreach (FileItem file in Files.Filtered)
            {
                file.IsSelected = !file.IsSelected;
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
                MessageBox.Show("Please, select path");
                return;
            }
            
            if (dialog.SelectedPath.Contains(App.Args["--path"]))
            {
                MessageBox.Show("Please, select another path");
                return;
            }
            
            Parallel.ForEach(Files.GetSelectedFiles(), item =>
            {
                File.Move(Path.GetFullPath(item.Path),dialog.SelectedPath + Path.DirectorySeparatorChar + item.FullName, true);
                App.Current.Dispatcher.BeginInvoke(() => Files.Source.Remove(item));
            });
        }
        
        [RelayCommand]
        private void DeleteSelected()
        {
            Parallel.ForEach(Files.GetSelectedFiles(), item =>
            {
                File.Delete(Path.GetFullPath(item.Path));
                App.Current.Dispatcher.BeginInvoke(() => Files.Source.Remove(item));
            });
            
            Files.Filtered.Refresh();
        }
    }
}