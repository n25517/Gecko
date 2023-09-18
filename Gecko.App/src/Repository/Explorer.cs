using System.IO;
using Gecko.App.Model;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace Gecko.App.Repository
{
    public static class Explorer
    {
        private static IEnumerable<string> GetFilesOnPath(string path, string pattern = "*") => Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories);
        
        public static ICollectionView GetFileItemsOnPath(string path, string pattern = "*")
        {
            var files = new List<FileItem>();
            foreach (var file in GetFilesOnPath(path, pattern))
            {
                files.Add(new FileItem
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    FullName = Path.GetFileName(file),
                    Extension = Path.GetExtension(file)
                });
            }
            
            return CollectionViewSource.GetDefaultView(files);
        }
    }
}