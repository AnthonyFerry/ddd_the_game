using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : MonoBehaviour {

    private Vector3 _destination; // in World
    private Vector2 _gridDest; // on GameBoard
    [SerializeField]
    private bool _hasADestination;
    private bool _isMoving;
    public float moveSpeed = 5.0f;
    public BasePawn _parent;
    public LevelTerrain _gameBoard;
    public Vector3 _wantedLocation;//mid-move location

    public void Init(BasePawn parent, LevelTerrain board)
    {
        _parent = parent;
        _hasADestination = false;
        _isMoving = false;
        _gameBoard = board;
    }

    public Vector2 Destination
    {
        get { return _destination; }
    }

    public bool HasDestination
    {
        get { return _hasADestination; }
    }
    
    public bool InternalUpdate()
    {
        if (_isMoving)
        {
            if (_parent.transform.position == _wantedLocation)
            {
                Debug.Log("Target acquierd, i said target acquiered. Stop moving.");
                _isMoving = false;
            }
            else
            {
                _parent.transform.position = Vector3.MoveTowards(_parent.transform.position, _wantedLocation, Time.deltaTime * moveSpeed);
            }
            
            return false;
        }

        // On-the-fly Pathfinding
        if (_hasADestination)
        {
            if (_destination == _parent.WorldPosition) {
                Debug.Log("Party's over, we reached destination");
                //_gameBoard.GetCellByPosition(_gridDest).SetState(CellState.occupied);
                _destination = Vector3.zero;
                _parent.hasAlreadyMoved = true;
                _hasADestination = false;
                _gameBoard.refreshOccupationMap();
                if (!_gameBoard.searchForTargets(_gameBoard.GetCellByPosition(_parent.PawnLocation), _parent.attackRange, _parent.attackType, _parent.isPlayer))
                {
                    GameManager.Instance.TurnTransition();
                }
                else
                {
                    Debug.Log("Blablabla !");
                    _gameBoard.refreshOccupationMap();
                    _gameBoard.HighlightTargetables(_gameBoard.GetCellByPosition(_parent.PawnLocation), _parent.attackRange, _parent.attackType, _parent.isPlayer);
                    if (_parent.isPlayer) {
                        _gameBoard._currentSelectedPawn = _parent;
                    } else {
                        EnemyIntel.Instance.openTimer(ActualStep.afterMoving);
                    }
                }
                return false;
            }

            Vector2 simplifiedDest = new Vector2(_destination.x, _destination.z);
            Vector2 simplifiedPos = new Vector2(_parent.WorldPosition.x, _parent.WorldPosition.z);
            Vector2 simplifiedGridPos = _parent.PawnLocation;
            
            var distance = (simplifiedDest - simplifiedPos).magnitude; // refers to World
            //Debug.Log(distance);

            // -> Find near box and get distance from destination --
            var locationsToTest = new List<Box>(9);
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    // Ignore the center 
                    if (i == 0 && j == 0)
                        continue;

                    var box = new Box(); // refers to Grid
                    box.Location = simplifiedGridPos + new Vector2(i, j);
                    box.Distance = (_gridDest - box.Location).magnitude;

                    // Ignore boxes that are too far
                    //if (box.Distance > distance + 0.5f)
                    //continue;

                    locationsToTest.Add(box);
                }
            }

            // -> Get the nearest, test it, remove it, retry --
            while (locationsToTest.Count > 0)
            {
                float minValue = -1;
                int minId = -1;

                for (int i = 0; i < locationsToTest.Count; i++)
                {
                    var val = locationsToTest[i].Distance;
                    if (val < minValue || minValue == -1)
                    {
                        minId = i;
                        minValue = val;
                    }
                }

                var nearest = locationsToTest[minId];
                if (TryMoveTo(nearest.Location))
                {
                    //Debug.Log("return true ?");
                    return true;
                }
                else
                {
                    //Debug.Log("Remove nearest : "+nearest.Location);
                    locationsToTest.Remove(nearest);
                }
            }

            // If we reach this point, no boxes worked : stop trying, because it won't work Jessica ! Stop lying to yourslef, and disable that goddamn destination !
            //Debug.Log("it won't work Jessica, let it go...");
            DisableDestination();
        }

        return false;
    }

    //WORK IN PROGRESS
    public Cell findNearestDestination(Vector2 dest) { //Dest refers to the board

        Cell tested = null;
        Cell nearest = null;
        bool isPairColumn = (dest.y % 2 == 0);
        Vector2 mod;
        int k = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                if (isPairColumn) {
                    if (i > 0 && (j == -1 || j == 1))
                        continue;
                } else {
                    if (i < 0 && (j == -1 || j == 1))
                        continue;
                }

                mod = new Vector2(i, j);
                tested = _gameBoard.GetCellByPosition(dest + mod);

                if (tested == null)
                    continue;

                if (tested.GetState() != CellState.neighbourg)
                    continue;
                //_neighbours.Add(tested);

                if (k == 0 || (tested.BoardPosition - dest).magnitude < (nearest.BoardPosition - dest).magnitude) {
                    nearest = tested;
                }

                k += 1;
            }
        }

        return nearest;

    }

    public void NewDestination(Cell dest) //Dest refers to the board
    {
        //Debug.Log("Enter NewDestination of "+_parent.name);
        _destination = dest.gameObject.transform.position;
        _gridDest = dest.BoardPosition;
        _hasADestination = true;
        //Debug.Log("Exit NewDestination of " + _parent.name);
    }

    private void DisableDestination()
    {
        _hasADestination = false;
    }

    void MoveTo(Vector3 wantedLocation)
    {
        Debug.Log("move Jessica, move to "+wantedLocation);
        //_translation.MoveTo(wantedLocation);

        _isMoving = true;
        _wantedLocation = wantedLocation;
    }

    bool TryMoveTo(Vector2 coord)// coord from Board
    {
        var env = GetNeighborhood(_parent.PawnLocation, coord); // coord from GameBoard
        Debug.Log(env.TargetedIndex);
        // Is the location accessible --
        if (env.TargetedIndex == -1)
        {
            return false;
        }

        if (env.OccupationMap[env.TargetedIndex])
        {
            switch (env.TargetedIndex)
            {
                case 0:
                    _parent.updateLocation(0, -1);
                    break;
                case 1:
                    _parent.updateLocation(1, -1);
                    break;
                case 2:
                    _parent.updateLocation(1, 0);
                    break;
                case 3:
                    _parent.updateLocation(1, 1);
                    break;
                case 4:
                    _parent.updateLocation(0, 1);
                    break;
                case 5:
                    _parent.updateLocation(-1, 1);
                    break;
                case 6:
                    _parent.updateLocation(-1, 0);
                    break;
                case 7:
                    _parent.updateLocation(-1, -1);
                    break;
                default:
                    Debug.Log("Switch error in TryMoveTo belonging to " + _parent.name);
                    break;
            }
            Vector3 cellWorldPos = _gameBoard.GetCellByPosition(coord).gameObject.transform.position;
            //MoveTo(new Vector3(cellWorldPos.x, 0.5f, cellWorldPos.y));//
            MoveTo(cellWorldPos);
            return true;
        }

        return false;
    }

    Neighborhood GetNeighborhood(Vector2 parent, Vector2 dest) //both from Grid
    {
        //Debug.Log("In GetNeighborhood, parent = " + parent + " and dest = " + dest);
        var result = new Neighborhood();
        result.OccupationMap = new bool[8];
        result.TargetedIndex = -1;
        Vector2 toCheck = parent;
        Cell cellToCheck = null;
        bool isPair = true; //to check if the parent's location is on a pair column (true) or not (false)

        if (parent.y % 2 != 0)
            isPair = false;

        toCheck.x = parent.x;
        toCheck.y = parent.y - 1;
        if (toCheck.x == dest.x && toCheck.y == dest.y)
            result.TargetedIndex = 0;
        cellToCheck = _gameBoard.GetCellByPosition(toCheck);
        result.OccupationMap[0] = false;
        if (cellToCheck != null) {
            if (_parent.PawnType != "Winged") {
                result.OccupationMap[0] = cellToCheck.isAccessible;
            } else {
                result.OccupationMap[0] = cellToCheck.isFlyable;
            }
        }

        toCheck.x = parent.x + 1;
        toCheck.y = parent.y;
        if (toCheck.x == dest.x && toCheck.y == dest.y)
            result.TargetedIndex = 2;
        cellToCheck = _gameBoard.GetCellByPosition(toCheck);
        result.OccupationMap[2] = false;
        if (cellToCheck != null)
        {
            if (_parent.PawnType != "Winged")
            {
                result.OccupationMap[2] = cellToCheck.isAccessible;
            }
            else {
                result.OccupationMap[2] = cellToCheck.isFlyable;
            }
        }

        toCheck.x = parent.x;
        toCheck.y = parent.y + 1;
        if (toCheck.x == dest.x && toCheck.y == dest.y)
            result.TargetedIndex = 4;
        cellToCheck = _gameBoard.GetCellByPosition(toCheck);
        result.OccupationMap[4] = false;
        if (cellToCheck != null)
        {
            if (_parent.PawnType != "Winged")
            {
                result.OccupationMap[4] = cellToCheck.isAccessible;
            }
            else {
                result.OccupationMap[4] = cellToCheck.isFlyable;
            }
        }

        toCheck.x = parent.x - 1;
        toCheck.y = parent.y;
        if (toCheck.x == dest.x && toCheck.y == dest.y)
            result.TargetedIndex = 6;
        cellToCheck = _gameBoard.GetCellByPosition(toCheck);
        result.OccupationMap[6] = false;
        if (cellToCheck != null)
        {
            if (_parent.PawnType != "Winged")
            {
                result.OccupationMap[6] = cellToCheck.isAccessible;
            }
            else {
                result.OccupationMap[6] = cellToCheck.isFlyable;
            }
        }

        result.OccupationMap[1] = false;
        result.OccupationMap[3] = false;
        result.OccupationMap[5] = false;
        result.OccupationMap[7] = false;
        
        if (isPair) {
            result.OccupationMap[1] = false;
            result.OccupationMap[3] = false;

            toCheck.x = parent.x - 1;
            toCheck.y = parent.y - 1;
            if (toCheck.x == dest.x && toCheck.y == dest.y)
                result.TargetedIndex = 7;
            cellToCheck = _gameBoard.GetCellByPosition(toCheck);
            if (cellToCheck != null)
            {
                if (_parent.PawnType != "Winged")
                {
                    result.OccupationMap[7] = cellToCheck.isAccessible;
                }
                else {
                    result.OccupationMap[7] = cellToCheck.isFlyable;
                }
            }

            toCheck.x = parent.x - 1;
            toCheck.y = parent.y + 1;
            if (toCheck.x == dest.x && toCheck.y == dest.y)
                result.TargetedIndex = 5;
            cellToCheck = _gameBoard.GetCellByPosition(toCheck);
            if (cellToCheck != null)
            {
                if (_parent.PawnType != "Winged")
                {
                    result.OccupationMap[5] = cellToCheck.isAccessible;
                }
                else {
                    result.OccupationMap[5] = cellToCheck.isFlyable;
                }
            }
        }
        else
        {
            result.OccupationMap[5] = false;
            result.OccupationMap[7] = false;

            toCheck.x = parent.x + 1;
            toCheck.y = parent.y - 1;
            if (toCheck.x == dest.x && toCheck.y == dest.y)
                result.TargetedIndex = 1;
            cellToCheck = _gameBoard.GetCellByPosition(toCheck);
            if (cellToCheck != null)
            {
                if (_parent.PawnType != "Winged")
                {
                    result.OccupationMap[1] = cellToCheck.isAccessible;
                }
                else {
                    result.OccupationMap[1] = cellToCheck.isFlyable;
                }
            }

            toCheck.x = parent.x + 1;
            toCheck.y = parent.y + 1;
            if (toCheck.x == dest.x && toCheck.y == dest.y)
                result.TargetedIndex = 3;
            cellToCheck = _gameBoard.GetCellByPosition(toCheck);
            if (cellToCheck != null)
            {
                if (_parent.PawnType != "Winged")
                {
                    result.OccupationMap[3] = cellToCheck.isAccessible;
                }
                else {
                    result.OccupationMap[3] = cellToCheck.isFlyable;
                }
            }
        }
        
        return result;
    }

    #region Structs
    private struct Box
    {
        public Vector2 Location; //on Grid
        public float Distance; //from Grid
    }

    private struct Neighborhood
    {
        //  [7][0][1]
        //  [6] p [2]
        //  [5][4][3]
        // Each cell of OccupationMap contains the state of a surrounding position (p is for ParentPosition (arg1 of TryMoveTo)).
        // If the value of a cell is True, it means that the associated position is free and walkable.
        // Depending on tht position of "p" in the array, the column it is in, the cells 1 & 3 or 7 & 5 will automatically be setted to False.
        public bool[] OccupationMap;
        public int TargetedIndex;

    }
    #endregion
}
