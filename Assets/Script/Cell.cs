using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public Color freeColor, occupiedColor, clickedColor, neighbourgColor, noneColor, blockColor;

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
            return GetState() != CellState.none && GetState() != CellState.block && GetState() != CellState.occupied && GetState() != CellState.clicked;
        }
    }

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
        if(state == CellState.free)
        {
            GetComponent<SpriteRenderer>().color = freeColor;
        }
        else if (state == CellState.occupied)
        {
            GetComponent<SpriteRenderer>().color = occupiedColor;
        }
        else if (state == CellState.clicked)
        {
            GetComponent<SpriteRenderer>().color = clickedColor;
        }
        else if (state == CellState.neighbourg)
        {
            GetComponent<SpriteRenderer>().color = neighbourgColor;
        }
        else if (state == CellState.none)
        {
            GetComponent<SpriteRenderer>().color = noneColor;
        }
        else if (state == CellState.block)
        {
            GetComponent<SpriteRenderer>().color = blockColor;
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
    clicked,
    neighbourg,
    none,
    block
}
