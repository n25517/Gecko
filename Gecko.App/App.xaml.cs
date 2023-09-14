using System;
using System.Windows;
using System.Collections.Generic;

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
                MessageBox.Show("Path is not specified");
                this.Shutdown();
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