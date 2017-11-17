using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelecter : MonoBehaviour {

    [SerializeField]
    Image _picture;

    [SerializeField]
    Text _name;

    public int selectedPawn = 0;

    public void SelecteNextPawn()
    {
        selectedPawn = selectedPawn + 1 > 4 ? 0 : selectedPawn + 1;

        RefreshDisplay();
    }

    public void SelectPawn(int index)
    {
        selectedPawn = Mathf.Clamp(index, 0, 4);

        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        var tm = TeamManager.Instance;

        _picture.sprite = tm.availablePawns[selectedPawn].Picture;
        _name.text = tm.availablePawns[selectedPawn].Type;
    }

    void Start()
    {
        selectedPawn = 2;

        RefreshDisplay();
    }
}
