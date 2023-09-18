using System.IO;
using Gecko.App.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gecko.App.Repository
{
    public static class Explorer
    {
        private static IEnumerable<string> GetFilesOnPath(string path, string pattern = "*") => Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories);
        
        public static ObservableCollection<FileItem> GetFileItemsOnPath(string path, string pattern = "*")
        {
            var files = new ObservableCollection<FileItem>();
            foreach (var file in GetFilesOnPath(path, pattern))
            {
                files.Add(new FileItem
                {
                    Path = file,
                    Name = Path.GetFileNameWithoutExtension(file),
                    FullName = Path.GetFileName(file),
                    Extension = Path.GetExtension(file)
                });
            }
            
            return files;
        }
    }
}