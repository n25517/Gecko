using System;
using System.Windows;
using System.Collections.Generic;
using Forms = System.Windows.Forms;

namespace Gecko.App
{
    public partial class App : Application
    {
        public static readonly Dictionary<string, string> Args = new();
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.PrepareArguments(e);

            if (!Args.ContainsKey("--path"))
            {
                using (var dialog = new Forms.FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() != Forms.DialogResult.OK)
                    {
                        this.Shutdown();
                    }
                    
                    Args.Add("--path", dialog.SelectedPath);
                }
            }
        }

        private void PrepareArguments(StartupEventArgs e)
        {
            foreach (var arg in e.Args)
            {
                if (arg.StartsWith("--"))
                {
                    var kv = arg.Split("=");
                    if (kv.Length == 2)
                    {
                        Args.Add(kv[0], kv[1]);
                    }
                }
            }
        }
    }
}