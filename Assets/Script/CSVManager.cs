using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CommonTools
{

    public class CSVManager {

        /// <summary>
        /// The datas contained in the CSV file.
        /// </summary>
        string[,] _datas;

        /// <summary>
        /// Dimensions of the CSV file.
        /// </summary>
        int _columnCount, _lineCount;


        public int ColumnCount { get { return _columnCount; } }
        public int LineCount { get { return _lineCount; } }

        public string this[int i, int j]
        {
            get { return _datas[i, j]; }
        }

        public CSVManager() { }
        public CSVManager(string file, bool isFilePath = true)
        {
            if (isFilePath)
                LoadDatasFromFile(file);
            else
                LoadDatasFromTextFile(file);
        }

        public CSVManager(StreamReader reader)
        {
            LoadDatasFromStream(reader);
        }

        public void LoadDatasFromTextFile(string file)
        {
            if (file == null)
                return;

            string[] lines = file.Split('\n');
            _lineCount = lines.Length;

            for (int i = 0; i < _lineCount; i++)
            {
                string[] cols = lines[i].Split(',');

                if (_columnCount == 0)
                {
                    _columnCount = cols.Length;
                    _datas = new string[_lineCount, _columnCount];
                }

                for (int j = 0; j < _columnCount; j++)
                {
                    _datas[j, i] = cols[j];
                }
            }
        }






        public void LoadDatasFromStream(StreamReader reader)
        {
            if (reader == null)
                return;

            string line;
            List<string> lines = new List<string>();

            using (reader)
            {
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        lines.Add(line);
                    }
                }
                while (line != null);
                reader.Close();
            }

            int width = 0, i = 0, j = 0;

            for (j = 0; j < lines.Count; j++)
            {
                string[] row = lines[j].Split(',');
                if (width == 0)
                {
                    width = row.Length;
                    _datas = new string[width, lines.Count];
                }

                for (i = 0; i < width; i++)
                    _datas[i, j] = row[i];
            }

            _columnCount = width;
            _lineCount = lines.Count;
        }

        public void LoadDatasFromFile(string filePath)
        {
            if (filePath == null)
                return;

            string line;
            StreamReader reader = new StreamReader(filePath);
            List<string> lines = new List<string>();

            using (reader)
            {
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        lines.Add(line);
                    }
                }
                while (line != null);
                reader.Close();
            }

            int width = 0, i = 0, j = 0;

            for (j = 0; j < lines.Count; j++)
            {
                string[] row = lines[j].Split(',');
                if (width == 0)
                {
                    width = row.Length;
                    _datas = new string[width, lines.Count];
                }

                for (i = 0; i < width; i++)
                    _datas[i, j] = row[i];
            }

            _columnCount = width;
            _lineCount = lines.Count;        
        }

        public void WriteFile(string path, string datas)
        {
        
        }

        public string[,] GetDataArray()
        {
            return _datas;
        }

        Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

}