using System;

namespace PDCore.Database
{
    /// <summary>
    /// Abstrakte Kommunikationsklasse stellt Basisfunktionalität zur Datenbankanbindung bereit
    /// </summary>
    public abstract class Communicator
    {

        private string m_connectionString;

        public string ConnectionString
        {
            get { return m_connectionString; }
            protected set
            {
                m_connectionString = value;
            }
        }

        public delegate void ExHandler(MessageType mType, Exception Message);
        public event ExHandler MessageThrown;

        protected virtual void OnMessageThrown(MessageType mType, Exception ex)
        {
            if (MessageThrown != null)
                MessageThrown(mType, ex);
        }

        public delegate void ExHandler2();
        public event ExHandler2 DatabaseUnreachable;

        protected void OnUnreachableThrown()
        {
            if (DatabaseUnreachable != null)
                DatabaseUnreachable();
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        public abstract bool checkConnection();

        public abstract long executeQuery(string query);

        public enum MessageType
        { 
            Warning,
            Error,
            Information
        }


    }
}
