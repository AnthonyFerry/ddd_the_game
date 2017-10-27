using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    LevelTerrain _terrain;
    CSVManager _reader;

	void Start () {
        _terrain = GetComponent<LevelTerrain>();

        if (_terrain == null)
        {
            Debug.LogError("Le component LevelTerrain n'existe pas sur l'objet " + gameObject.name);
            return;
        }

        _reader = new CSVManager();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
        {
            string fileData = "";

            List<Cell> cells = _terrain.Cells;
            int width = _terrain.Width;
            int height = _terrain.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell currentCell = _terrain.GetCellByPosition(j, i);
                    string state;

                    switch (currentCell.GetState())
                    {
                        case CellState.none:
                            state = "-1";
                            break;

                        case CellState.free:
                            state = "0";
                            break;

                        case CellState.block:
                            state = "1";
                            break;

                        default:
                            state = "0";
                            break;
                    }


                    fileData += j == width - 1 ? state : state + ','; 
                }

                fileData += '\n';
            }

            _reader.WriteFile("C:/Users/Anthony Ferry/Desktop/TestDeMesCouilles.csv", fileData);
            
        }
	}
}


