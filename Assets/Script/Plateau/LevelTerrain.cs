using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;

#if UNITY_EDITOR
using UnityEditor;

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
    const int FIRST_PAWN = 100;
    const int SECOND_PAWN = 101;
    const int THIRD_PAWN = 102;
    const int FOURTH_PAWN = 103;
    const int FIFTH_PAWN = 104;
    

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

        _terrainWidth = _reader.ColumnCount;
        _terrainHeight = _reader.LineCount;
    }

    int[,] LoadCSVDatasAsIntArray()
    {
        TextAsset file = (TextAsset)Resources.Load(MenuDatas.Instance.selectedLevel) as TextAsset;
        string fileText = file.text;

        Debug.Log(fileText);

        if (_reader == null) _reader = new CSVManager(fileText, false);

        int[,] intDatas = new int[_reader.ColumnCount, _reader.LineCount];

        for (int i = 0; i < _reader.LineCount; i++)
            for (int j = 0; j < _reader.ColumnCount; j++)
                intDatas[i, j] = int.Parse(_reader[i, j]);

        return intDatas;
    }

    public void BuildTerrain(int terrainNumber) {
        _terrainNumber = terrainNumber;

        // Clear the terrain if there is any 
        if (!isEmpty)
            ClearTerrain();

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

                    case FIRST_PAWN:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                TeamManager.Instance.playerTeam[0], newCell.transform.position, new Vector2(i, j), true
                            )
                        );
                        break;

                    case SECOND_PAWN:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                TeamManager.Instance.playerTeam[1], newCell.transform.position, new Vector2(i, j), true
                            )
                        );
                        break;

                    case THIRD_PAWN:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                TeamManager.Instance.playerTeam[2], newCell.transform.position, new Vector2(i, j), true
                            )
                        );
                        break;

                    case FOURTH_PAWN:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                TeamManager.Instance.playerTeam[3], newCell.transform.position, new Vector2(i, j), true
                            )
                        );
                        break;

                    case FIFTH_PAWN:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                TeamManager.Instance.playerTeam[4], newCell.transform.position, new Vector2(i, j), true
                            )
                        );
                        break;

                    case 200:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                _manager.GetPawnData(0), newCell.transform.position, new Vector2(i, j), false
                            )
                        );
                        break;

                    case 201:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                _manager.GetPawnData(1), newCell.transform.position, new Vector2(i, j), false
                            )
                        );
                        break;

                    case 202:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                _manager.GetPawnData(2), newCell.transform.position, new Vector2(i, j), false
                            )
                        );
                        break;

                    case 203:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                _manager.GetPawnData(3), newCell.transform.position, new Vector2(i, j), false
                            )
                        );
                        break;

                    case 204:
                        newCell.SetState(CellState.free);
                        _lstCells.Add(newCell);
                        _manager.createPawn(
                            _manager.createPawnData(
                                _manager.GetPawnData(4), newCell.transform.position, new Vector2(i, j), false
                            )
                        );
                        break;

                    default:
                        newCell.SetState(CellState.block);
                        break;
                }

                _lstCells.Add(newCell);
            }
        }


        transform.position = new Vector3(-_terrainWidth / 2, 0.2f, _terrainHeight / 2);
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
    }

    void HighlightNeighbourgs(Cell selectedCase, int index, bool isWinged)
    {
        if (selectedCase.GetState() == CellState.free) {
            selectedCase.SetState(CellState.neighbourg);
            _neighbourgs.Add(selectedCase);
        } /*else if (selectedCase.GetState() == CellState.occupied) {
            Debug.Log("occupied cell detected");
            if (_manager.isPawnExisting(selectedCase.BoardPosition)) {
                Debug.Log("Pawn detected on this cell");
                selectedCase.SetState(CellState.attackable);
            }
        }*/ else if (selectedCase.GetState() == CellState.selected) {
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
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
            if (neighbourg.isAccessible || (isWinged && neighbourg.isFlyable))
            {
                //neighbourg.SetState(CellState.neighbourg);
                //_neighbourgs.Add(neighbourg);

                HighlightNeighbourgs(neighbourg, index - 1, isWinged);
            }
            neighbourg = null;
        }
    }

    void HighlightTargetables(Cell selectedCase, int index, AtkSys atk)
    {
        
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if(neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    neighbourg.SetState(CellState.attackable);
                }

                
            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                HighlightTargetables(neighbourg, index - 1, atk);
            neighbourg = null;
        }
    }

    public bool searchForTargets(Cell selectedCase, int index, AtkSys atk)
    {

        if (index == 0) return false; //If we reach this point, we didn't find any good target.

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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if(searchForTargets(neighbourg, index - 1, atk)) return true;
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if (searchForTargets(neighbourg, index - 1, atk)) return true;
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if (searchForTargets(neighbourg, index - 1, atk)) return true;
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if (searchForTargets(neighbourg, index - 1, atk)) return true;
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if (searchForTargets(neighbourg, index - 1, atk)) return true;
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
            if (neighbourg.GetState() == CellState.occupied)
            {
                BasePawn target = _manager.getPawnByLocation(neighbourg.BoardPosition);
                if (target != null && !target.isPlayer)
                {
                    //neighbourg.SetState(CellState.attackable);
                    return true;
                }


            }

            if (neighbourg.isAccessible || atk == AtkSys.Ranged)
                if (searchForTargets(neighbourg, index - 1, atk)) return true;
            neighbourg = null;
        }

        return false;
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
        BuildTerrain(3);
        refreshOccupationMap();
    }

    void FixedUpdate()
    {
        if (_manager.Locker != GameState.unlocked || !_manager.IsPlayerTurn) return;

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

                    if (_currentSelectedPawn == selectedPawn) return;
                    
                    Cell selectedCell = GetCellByPosition(selectedPawn.PawnLocation);

                    if (selectedPawn.isPlayer) {
                        if(_manager.currentlyPlayedPawn == null || _manager.currentlyPlayedPawn == selectedPawn || (!_manager.currentlyPlayedPawn.hasAlreadyMoved && _manager.currentlyPlayedPawn != selectedPawn)) {
                            _currentSelectedPawn = selectedPawn;
                            _manager.currentlyPlayedPawn = _currentSelectedPawn;
                        } else {
                            return;
                        }
                        
                    } else {
                        if(selectedCell.GetState() == CellState.attackable)
                            //if (!selectedPawn.takeDamages(_currentSelectedPawn.dealDamages(selectedPawn.PawnType)))
                            if (!_currentSelectedPawn.atkFunction(selectedPawn))
                                _manager.destroyPawn(selectedPawn);
                        _currentSelected = null;
                        _currentSelectedPawn = null;
                        ClearNeighbourgs();
                        refreshOccupationMap();
                        return;
                    }

                    if (!selectedPawn.isPlayer) return;

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
                        if (!selectedPawn.hasAlreadyMoved) HighlightNeighbourgs(_currentSelected, selectedPawn.moveRange, selectedPawn.PawnType == "Winged");
                        HighlightTargetables(_currentSelected, selectedPawn.attackRange, _currentSelectedPawn.attackType);
                    }
                } else if (hit.collider.GetComponent<Cell>() != null) {
                    Cell selectedCell = hit.collider.GetComponent<Cell>();
                    Debug.Log(selectedCell.gameObject.name + " selected");
                    if (selectedCell.GetState() == CellState.neighbourg)
                    {
                        _currentSelected.SetState(CellState.free);
                        _manager.currentlyPlayedPawn = _currentSelectedPawn;
                        _currentSelectedPawn.moveFunction(selectedCell);
                        selectedCell = null;
                        _currentSelectedPawn = null;
                        ClearNeighbourgs();
                        //refreshOccupationMap();
                    }
                }
            }
        }
    }
}
