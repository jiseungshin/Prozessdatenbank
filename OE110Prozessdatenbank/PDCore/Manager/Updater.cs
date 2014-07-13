using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace PDCore.Manager
{
    public class Updater
    {
                 #region instancing

        static Updater instance = null;
        static readonly object padlock = new object();

        Updater()
        {
            initUpdateWatcher();
        }

        public static Updater Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Updater();
                    }
                    return instance;
                }
            }
        }

        #endregion

        public delegate void UpdateHandler(params string[] values);
        public event UpdateHandler newData;

        private void OnUpdateTrigger(params string[] values)
        {
            if (newData != null)
                newData(values);
        }

        //public void forceUpdate()
        //{
        //    OnUpdateTrigger();
        //}

        public void forceUpdate(params string[] values)
        {

            try
            {
                IO.SimpleIO.saveClearText(@"update\upd.txt", new List<string>() { DateTime.Now.ToString() });
            }
            catch
            {

                OnUpdateTrigger();
            }
        }

        FileSystemWatcher watcher;
        private void initUpdateWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = @"update\\";
            watcher.Filter = "upd.txt";

            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }
        int i = 0;
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            OnUpdateTrigger();
            //if (i == 1)
            //{
            //    //System.Windows.MessageBox.Show(i.ToString());
            //    OnUpdateTrigger();
            //    i = 0;
            //}
            //else
            //    i++;

            
        }
    }
}
