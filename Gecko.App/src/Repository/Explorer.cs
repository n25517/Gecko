using System;
using System.IO;
using System.Linq;
using Gecko.App.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gecko.App.Repository
{
    public static class Explorer
    {
        private static IEnumerable<string> GetFilesOnPath(string path, string pattern = "*") => SafeEnumerateFiles(path, pattern, SearchOption.AllDirectories);
        
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
        
        
        
        // https://stackoverflow.com/a/20719754/12497422
        private static IEnumerable<string> SafeEnumerateFiles(string rootPath, string patternMatch, SearchOption searchOption)
        {
            var files = Enumerable.Empty<string>();
            if (searchOption == SearchOption.AllDirectories)
            {
                try
                {
                    var subDirs = Directory.EnumerateDirectories(rootPath);
                    foreach (var dir in subDirs)
                    {
                        files = files.Concat(SafeEnumerateFiles(dir, patternMatch, searchOption));
                    }
                }
                catch (PathTooLongException) { }
                catch (UnauthorizedAccessException) { }
            }

            try
            {
                files = files.Concat(Directory.EnumerateFiles(rootPath, patternMatch));
            }
            catch (UnauthorizedAccessException) { }

            return files;
        }
    }
}