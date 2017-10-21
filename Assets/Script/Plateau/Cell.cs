using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public Color freeColor, occupiedColor, clickedColor, neighbourgColor, noneColor, blockColor;

    [SerializeField]
    int _x, _y;

    public int x
    {
        get { return _x; }
    }

    public int y
    {
        get { return _y; }
    }

    public bool isAccessible
    {
        get
        {
            return GetState() != CellState.none && GetState() != CellState.block && GetState() != CellState.occupied && GetState() != CellState.selected;
        }
    }

    [SerializeField]
    CellState _state;

    public void SetCellCoordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void SetState(CellState newState)
    {
        _state = newState;
        ChangeCaseColor(newState);
    }

    public CellState GetState()
    {
        return _state;
    }

    void ChangeCaseColor(CellState state)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        if (renderer == null)
        {
            Debug.LogError("Le comportement SpriteRenderer n'a pas été trouvé");
            return;
        }

        switch(state)
        {
            case CellState.none:
                renderer.color = noneColor;
                break;

            case CellState.free:
                renderer.color = freeColor;
                break;

            case CellState.block:
                renderer.color = blockColor;
                break;

            case CellState.selected:
                renderer.color = clickedColor;
                break;

            case CellState.occupied:
                renderer.color = occupiedColor;
                break;

            case CellState.neighbourg:
                renderer.color = neighbourgColor;
                break;

            default:
                renderer.color = blockColor;
                break;
        }
    }

    void Start()
    {
    }
}

public enum CellState
{
    free,
    occupied,
    selected,
    neighbourg,
    none,
    block
}
