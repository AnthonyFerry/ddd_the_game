using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVManager {

    string[,] _datas;
    int _rowCount, _lineCount;

    public int RowCount { get { return _rowCount; } }
    public int LineCount { get { return _lineCount; } }
    public string this[int i, int j]
    {
        get { return _datas[i, j]; }
    }

    public CSVManager() { }
    public CSVManager(string filePath)
    {
        LoadDatasFromFile(filePath);
    }

    public void LoadDatasFromFile(string filePath)
    {
        _datas = null;

        string fileText = File.ReadAllText(filePath);
        
        string[] lines = fileText.Split('\n');

        // Récupération du nombre de ligne et de colonne du document.
        _lineCount = lines.Length;
        _rowCount = lines[0].Split(',').Length;

        _datas = new string[_rowCount, _lineCount];
        for (int i = 0; i < _rowCount; i++)
            for (int j = 0; j < _lineCount; j++)
            {
                string value = lines[j].Split(',')[i];
                _datas[i, j] = value;
            }
    }

    public void WriteFile(string path, string datas)
    {
        if (!File.Exists(path))
            File.Create(path);

        File.WriteAllText(path, datas);
    }

    public string[,] GetDataArray()
    {
        return _datas;
    }
}

