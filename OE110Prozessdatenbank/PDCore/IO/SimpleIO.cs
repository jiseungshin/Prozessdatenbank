using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;


namespace PDCore.IO
{
    public static class SimpleIO
    {

        /// <summary>
        /// Checks wheather a File exists by a given FilePath
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool checkFileExists(string Path)
        {
            if (File.Exists(Path))
                return true;
            else
                return false;
        }

        public static bool checkDirectoryExists(string Path)
        {
            if (Directory.Exists(Path))
                return true;
            else
                return false;
        }

        public static void createDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static List<string> getClearText(string Path)
        {
            List<string> m_currentList = new List<string>();

            if (File.Exists(Path))
            {
                using (var m_reader = new StreamReader(Path))
                {
                    while (!m_reader.EndOfStream)
                    {
                        m_currentList.Add(m_reader.ReadLine());
                    }
                    m_reader.Close();
                }
            }

            return m_currentList;
        }

        public static bool saveClearText(string Path, List<string> Data)
        {
                using (var m_writer = new StreamWriter(Path))
                {
                    foreach (string line in Data)
                    {
                        m_writer.WriteLine(line);
                    }
                    m_writer.Close();
                }

            return false;
        }

        public static void AppendText(string path, string text)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine(text);
                w.Flush();
            }
        }

        public static void serializeObject(object Object, string Path)
        {
            FileStream m_fileStream = null;

            try
            {
                //FileStream für die Datei erzeugen 
                m_fileStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

                //Das Objekt serialisieren 
                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                m_BinaryFormatter.Serialize(m_fileStream, Object);
            }
            finally
            {
                //Am ende noch den FileStream schliesen. 
                if (m_fileStream != null)
                {
                    m_fileStream.Flush();
                    m_fileStream.Close();
                }
            }
        }

        public static object getSerializedObject(string Path)
        {
            FileStream m_fileStream = null;

            //Objekt für die Rückgabe erstellen 
            object m_object = null;

            try
            {
                //FileStream für die Datei erzeugen 
                m_fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

                //Das Objekt deserialisiern 
                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                m_object = m_BinaryFormatter.Deserialize(m_fileStream);
            }
            finally
            {
                //Am ende noch den FileStream schliesen. 
                if (m_fileStream != null)
                {
                    m_fileStream.Close();
                }
            }

            return m_object;
        }

        public static void removeFile(string Path)
        { 
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
    }
}
