using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PDCore.Database
{
    public class MySQLCommunicator : Communicator
    {

        public MySQLCommunicator()
        {
            generateConnectionString();
        }

        private string m_server;
        private string m_database;
        private string m_user;
        private string m_password;

        new public event ExHandler MessageThrown;

        protected override void OnMessageThrown(MessageType mType, Exception ex)
        {
            if (MessageThrown != null)
            {
                MessageThrown(mType, ex);               
            }
        }


        #region get/set

        public string Server
        {
            get { return m_server; }
            set
            {
                m_server = value;
                generateConnectionString();
            }
        }

        public string Database
        {
            get { return m_database; }
            set
            {
                m_database = value;
                generateConnectionString();
            }
        }

        public string User
        {
            get { return m_user; }
            set
            {
                m_user = value;
                generateConnectionString();
            }
        }

        public string Password
        {
            get { return m_password; }
            set
            {
                m_password = value;
                generateConnectionString();
            }
        }

        #endregion

        public override bool checkConnection()
        {
            MySqlConnection m_con = null;
            try
            {
                m_con = new MySqlConnection(ConnectionString);
                m_con.Open();
                m_con.Close();
                Console.WriteLine("Connection established!");


            }
            catch (MySqlException myerror)
            {
                OnMessageThrown(MessageType.Error, myerror);
            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Close();
                }
            }
            return true;
            
        }

        public DataSet getDataSet(string query)
        {
            MySqlConnection m_mySqlConnection = null;
            MySqlDataAdapter m_mySqlDataAdapter = null;
            MySqlCommandBuilder m_mySqlCommandBuilder = null;

            try
            {
                m_mySqlConnection = new MySqlConnection(ConnectionString);

                m_mySqlDataAdapter = new MySqlDataAdapter(query, m_mySqlConnection);
                m_mySqlCommandBuilder = new MySqlCommandBuilder(m_mySqlDataAdapter);
                
                var _dataSet = new DataSet();
                m_mySqlDataAdapter.Fill(_dataSet);

                return _dataSet;
            }
            catch (MySqlException ex)
            {
                OnMessageThrown(MessageType.Error, ex);
                return null;
            }
            finally
            {
                if (m_mySqlConnection != null)
                {
                    m_mySqlConnection.Close();

                }
            }
        }

        public override long executeQuery(string query)
        {
            MySqlConnection m_mySqlConnection = null;
            MySqlCommand m_mySqlCommand = null;
        
            try
            {
                m_mySqlConnection = new MySqlConnection(ConnectionString);


                m_mySqlCommand = new MySqlCommand(query);
                m_mySqlCommand.Connection = m_mySqlConnection;
                m_mySqlConnection.Open();
                m_mySqlCommand.ExecuteNonQuery();

                long id = m_mySqlCommand.LastInsertedId;

                return id;

            }
            catch (MySqlException ex)
            {
                OnMessageThrown(MessageType.Error, ex);
                return -1;
            }
            finally
            {
                if (m_mySqlConnection != null)
                {
                    m_mySqlConnection.Close();
                    
                }
            }

        }

        public int getNumberOf(string query)
        {
            MySqlConnection m_mySqlConnection = null;
            MySqlCommand m_mySqlCommand = null;

            try
            {
                m_mySqlConnection = new MySqlConnection(ConnectionString);


                m_mySqlCommand = new MySqlCommand("SELECT COUNT(*) FROM " + query);
                m_mySqlCommand.Connection = m_mySqlConnection;
                m_mySqlConnection.Open();
                return Convert.ToInt32(m_mySqlCommand.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                OnMessageThrown(MessageType.Error, ex);
            }
            finally
            {
                if (m_mySqlConnection != null)
                {
                    m_mySqlConnection.Close();
                }
            }

            return 0;
        }

        public int getNextIndex(string Table, string Field)
        {
            MySqlConnection m_mySqlConnection = null;
            MySqlCommand m_mySqlCommand = null;

            try
            {
                m_mySqlConnection = new MySqlConnection(ConnectionString);

                m_mySqlCommand = new MySqlCommand("SELECT MAX(" + Field + ") FROM " + Table + ";");
                m_mySqlCommand.Connection = m_mySqlConnection;
                m_mySqlConnection.Open();
                object res = m_mySqlCommand.ExecuteScalar();
                //System.Windows.MessageBox.Show(res.ToString());
                if (res.ToString() == "")
                    res = 0;
                return Convert.ToInt32(res) + 1;
            }
            catch (MySqlException ex)
            {
                OnMessageThrown(MessageType.Error, ex);
            }
            finally
            {
                if (m_mySqlConnection != null)
                {
                    m_mySqlConnection.Close();
                }
            }

            return 0;
        }

        public bool executeTransactedQueries(List<string> queries)
        {
            MySqlTransaction _mySqlTransaction = null;
            MySqlConnection m_mySqlConnection = null;
            bool success = true;

            try
            {
                m_mySqlConnection = new MySqlConnection(ConnectionString);
                m_mySqlConnection.Open(); ;
                _mySqlTransaction = m_mySqlConnection.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = m_mySqlConnection;
                cmd.Transaction = _mySqlTransaction;

                foreach (string _query in queries)
                {
                    cmd.CommandText = _query;
                    cmd.ExecuteNonQuery();
                }

                _mySqlTransaction.Commit();

            }
            catch (MySqlException ex)
            {
                try
                {
                    _mySqlTransaction.Rollback();

                }
                catch (MySqlException ex1)
                {
                    OnMessageThrown(MessageType.Error, ex1);
                }

                OnMessageThrown(MessageType.Error, ex);
                success = false;

            }
            finally
            {
                if (m_mySqlConnection != null)
                {
                    m_mySqlConnection.Close();
                    
                }
            }

            if (success)
                return true;
            else
                return false;
        }
        
        protected void generateConnectionString()
        {
            ConnectionString = "SERVER=" + Server + ";DATABASE=" + Database + ";UID=" + User + ";PASSWORD=" + Password;
        }

        public bool valueExists(string Table, string Field, object Value)
        {
            int ValueFound = 1;

            {
                ValueFound = getDataSet("SELECT * from " + Table + " where " + Field + "='" + Value + "'").Tables[0].Rows.Count;
            }

            if (ValueFound > 0)
                return true;
            else
                return false;
        }
        private static bool m_hasConnection = false;

        public static bool hasConnection
        { get { return m_hasConnection; } }

        public static bool checkLoginData(string server, string database, string user, string password, out int ErrorCode)
        {
            ErrorCode = 0;
            string m_ConnectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + user + ";PASSWORD=" + password;

            MySqlConnection m_con = null;
            try
            {
                m_con = new MySqlConnection(m_ConnectionString);
                m_con.Open();
                Console.WriteLine("Connection established!");

                m_hasConnection = true;

            }
            catch (MySqlException ex)
            {
                ErrorCode = ((MySqlException)ex).Number;
                return false;
            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Close();
                }
            }
            return true;
        }

        public static string BuildUpdateQuery(string Table, List<ColumnValuePair> Values, ColumnValuePair condition)
        {

            string _query = "UPDATE " + Table + " SET ";

            foreach(var pair in Values)
            {
                _query += pair.Culumn + "=" + pair.Value.ToDBObject() + ", ";
            }

            _query = _query.Remove(_query.LastIndexOf(','), 1);

            _query += " WHERE " + condition.Culumn + "=" + condition.Value.ToDBObject();


            return _query;
        }

        public struct ColumnValuePair
        {
            public string Culumn { get; set; }
            public object Value { get; set; }
        }
        
    }

    public static class ObjectExtensions
    {
        public static string ToDBObject(this object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            else
                if (obj is bool)
                {
                    return obj.ToString();
                }
            if (obj is int || obj is double)
            {
                return obj.ToString().Replace(',', '.');
            }
            if (obj is DateTime)
            {
                return "'"+Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss")+"'";
            }
            else
            {
                return "'" + obj + "'";
            }

        }
    }   
}
