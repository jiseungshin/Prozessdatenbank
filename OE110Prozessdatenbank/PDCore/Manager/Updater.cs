using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Manager
{
    public class Updater
    {
                 #region instancing

        static Updater instance = null;
        static readonly object padlock = new object();

        Updater()
        {
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
            OnUpdateTrigger();
        }
    }
}
