using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelTerrain : MonoBehaviour {

    // Prefab de case de plateau.
    [Header("Board Case Prefab")]
    public GameObject Cell;

    // Valeur de débug pour la récursivité de recherche des cases atteignables.
    [Header("How Many Neighbourgs to check ?")]
    public int NeighbourgsCount = 4;

    /// <summary>
    /// Retourne vrai si le plateau ne comporte aucune case (si il n'est pas chargé)
    /// </summary>
    public bool isEmpty { get { return _lstCells.Count == 0; } }
    public List<Cell> Cells { get { return _lstCells; } }
    public int Width { get { return _terrainWidth; } }
    public int Height { get { return _terrainHeight; } }

    // Constantes pour les valeurs de cases.
    const int NONE_CASE = -1;
    const int FREE_CASE = 0;
    const int BLOCK_CASE = 1;

    /// <summary>
    /// Valeur de décalage vertical des cases du plateau.
    /// </summary>
    const float VERTICAL_INTERVAL = -0.867f;

    [Header("Currently selected cell and pawn")]
    [SerializeField]
    Cell _currentSelected;
    [SerializeField]
    BasePawn _currentSelectedPawn;

    /// <summary>
    /// Liste de toutes les cases du plateau.
    /// </summary>
    List<Cell> _lstCells = new List<Cell>();

    /// <summary>
    /// Liste des cases voisine à la case sélectionnée.
    /// </summary>
    [SerializeField]
    List<Cell> _neighbourgs = new List<Cell>();

    int _terrainNumber;
    int[,] _terrain;
    int _terrainWidth, _terrainHeight;

    CSVManager _reader;

    public GameManager _manager;

    /// <summary>
    /// Initialise les différents paramètre du terrain chargé.
    /// </summary>
    /// <param name="terrainNumber"></param>
	void InitializeTerrain() {

        _terrain = LoadCSVDatasAsIntArray();

        _terrainWidth = _reader.RowCount;
        _terrainHeight = _reader.LineCount;
    }

    int[,] LoadCSVDatasAsIntArray()
    {
        if (_reader == null) _reader = new CSVManager();

        // Value pour PC seulement
        string filePath = Application.streamingAssetsPath + "/Maps/Map" + _terrainNumber + ".csv";
        _reader.LoadDatasFromFile(filePath);

        int[,] intDatas = new int[_reader.RowCount, _reader.LineCount];

        for (int i = 0; i < _reader.LineCount; i++)
            for (int j = 0; j < _reader.RowCount; j++)
                intDatas[i, j] = int.Parse(_reader[i, j]);

        return intDatas;
    }

    /// <summary>
    /// Procedure d'initialisation du plateau pour Android car le chargement par CSV ne fonctionnait pas.
    /// </summary>
    void ForAndroid()
    {
        _terrainWidth = _terrainHeight = 9;
        _terrain = new int[9, 9];


        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _terrain[i, j] = FREE_CASE;
            }
        }

        _terrain[4, 0] = NONE_CASE;
        _terrain[4, 1] = NONE_CASE;
        _terrain[4, 2] = BLOCK_CASE;
        _terrain[4, 6] = BLOCK_CASE;
        _terrain[4, 7] = NONE_CASE;
        _terrain[4, 8] = NONE_CASE;

        _terrain[8, 1] = NONE_CASE;
        _terrain[8, 3] = NONE_CASE;
        _terrain[8, 5] = NONE_CASE;
        _terrain[8, 7] = NONE_CASE;
    }

    public void BuildTerrain(int terrainNumber) {
        _terrainNumber = terrainNumber;

        // Clear the terrain if there is any 
        if (!isEmpty)
            ClearTerrain();

//      ForAndroid();

        InitializeTerrain();
        
        for (int i = 0; i < _terrainWidth; i++) {
            for (int j = 0; j < _terrainHeight; j++) {
                GameObject go = Instantiate(Cell, transform);
                go.name = "Case [" + i + "," + j + "]";

                float newX, newY = 0f;
                newY = j * VERTICAL_INTERVAL;
                newX = j % 2 == 0 ? i : i + 0.5f;

                go.transform.position = new Vector3(newX, 0, newY);

                var newCell = go.GetComponent<Cell>();
                
                newCell.SetCellCoordinates(i, j);

                switch (_terrain[i,j])
                {
                    case FREE_CASE:
                        newCell.SetState(CellState.free);
                        break;

                    case NONE_CASE:
                        newCell.SetState(CellState.none);
                        break;

                    case BLOCK_CASE:
                        newCell.SetState(CellState.block);
                        break;

                    default:
                        newCell.SetState(CellState.block);
                        break;
                }

                _lstCells.Add(newCell);
            }
        }

        //refreshOccupationMap();
    }

    public Cell GetCellByPosition(Vector2 pos) {
        return GetCellByPosition((int) pos.x, (int) pos.y);
    }

    public Cell GetCellByPosition(int x, int y)
    {
        foreach (Cell c in _lstCells)
        {
            if (c.x == x && c.y == y)
                return c;
        }

        return null;
    }

    void ClearNeighbourgs()
    {
        foreach (Cell n in _neighbourgs)
        {
            n.SetState(CellState.free);
        }

        _neighbourgs.Clear();
        //refreshOccupationMap();
    }

    void HighlightNeighbourgs(Cell selectedCase, int index)
    {
        if (selectedCase.GetState() == CellState.free) {
            selectedCase.SetState(CellState.neighbourg);
            _neighbourgs.Add(selectedCase);
        } else if (selectedCase.GetState() == CellState.selected) {
            _neighbourgs.Add(selectedCase);
        }

        if (index == 0) return;

        // Sommes nous sur une ligne paire ou impaire ?
        int lineSpec = selectedCase.y % 2 == 0 ? -1 : 1;

        Cell neighbourg;

        //  ___ ___ ___
        // |___|___|___|
        // |_X_|_P_|___|
        // |___|___|___|
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x - 1, selectedCase.y))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }

        //  ___ ___ ___
        // |___|___|___|
        // |___|_P_|_X_|
        // |___|___|___|
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x + 1, selectedCase.y))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }

        //  ___ ___ ___
        // |___|_X_|___|
        // |___|_P_|___|
        // |___|___|___|
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x, selectedCase.y + 1))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }

        //  ___ ___ ___
        // |___|___|___|
        // |___|_P_|___|
        // |___|_X_|___|
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x, selectedCase.y - 1))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }

        //  ___ ___ ___
        // |_X_|___|_X_|
        // |___|_P_|___|
        // |___|___|___|  En fonction de lineSpec
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x + lineSpec, selectedCase.y + 1))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }

        //  ___ ___ ___
        // |___|___|___|
        // |___|_P_|___|
        // |_X_|___|_X_|  En fonction de lineSpec
        // 
        // P = Player, X = case à checker
        if (neighbourg = GetCellByPosition(selectedCase.x + lineSpec, selectedCase.y - 1))
        {
            if (neighbourg.isAccessible)
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1);
            }
            neighbourg = null;
        }
    }

    public void ClearTerrain()
    {
        if (!isEmpty)
        {
            foreach (Cell c in _lstCells)
                DestroyImmediate(c.gameObject);

            _lstCells.Clear();
        }
    }

    public void refreshOccupationMap() {
        if (_manager.Pawns.Count > 0) {
            Cell c;
            foreach (BasePawn p in _manager.Pawns) {
                c = GetCellByPosition(p.PawnLocation);
                if (c != null && c.GetState() != CellState.selected)
                    c.SetState(CellState.occupied);
            }
        }
    }

    void Start()
    {
        _reader = new CSVManager();
        BuildTerrain(3);
        refreshOccupationMap();
    }

    void FixedUpdate()
    {
        // Si on touche l'écran ou si on clique
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            Vector3 position;

            if (Input.touchCount > 0)
            {
                position = Input.GetTouch(0).position;
            }
            else
            {
                position = Input.mousePosition;
            }

            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider.GetComponent<BasePawn>() != null)
                {
                    BasePawn selectedPawn = hit.collider.GetComponent<BasePawn>();

                    Debug.Log(selectedPawn.gameObject.name + " selected");

                    if (!selectedPawn.isPlayer) return;

                    if (_currentSelectedPawn == selectedPawn) return;

                    _currentSelectedPawn = selectedPawn;

                    Cell selectedCell = GetCellByPosition((int)selectedPawn.PawnLocation.x, (int)selectedPawn.PawnLocation.y);

                    ClearNeighbourgs();
                    

                    if (selectedCell == _currentSelected)
                    {
                        Debug.Log("Cell under SelectedPawn. SelectedCell = "+selectedCell.name+" & currentSelected = "+_currentSelected.name);
                        selectedCell.SetState(CellState.free);
                        _currentSelected = null;
                        refreshOccupationMap();
                        
                    }
                    else
                    {
                        if (_currentSelected)
                            _currentSelected.SetState(CellState.free);

                        selectedCell.SetState(CellState.selected);
                        _currentSelected = selectedCell;

                        refreshOccupationMap();
                        HighlightNeighbourgs(_currentSelected, selectedPawn.moveRange);
                    }
                } else if (hit.collider.GetComponent<Cell>() != null) {
                    Cell selectedCell = hit.collider.GetComponent<Cell>();
                    Debug.Log(selectedCell.gameObject.name + " selected");
                    if (selectedCell.GetState() == CellState.neighbourg)
                    {
                        _currentSelected.SetState(CellState.free);
                        _currentSelectedPawn.moveFunction(selectedCell);
                        selectedCell = null;
                        _currentSelectedPawn = null;
                        ClearNeighbourgs();
                    }
                }
            }
        }
    }
}

[CustomEditor(typeof(LevelTerrain))]
public class LevelTerrainEditor : Editor
{ 
    public override void OnInspectorGUI()
    {

        LevelTerrain _lt = (LevelTerrain)target;

        int levelCount = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/", "*.csv", System.IO.SearchOption.AllDirectories).Length;

        EditorGUILayout.BeginHorizontal();
        for (int i = 1; i <= levelCount; i++)
        {
            string levelName = "lvl " + i;
            if (GUILayout.Button(levelName))
            {
                _lt.BuildTerrain(i);
                Debug.Log(i);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear level"))
        {
            _lt.ClearTerrain();
        }
        

        DrawDefaultInspector();
    }
}