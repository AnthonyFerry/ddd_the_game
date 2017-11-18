using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSelector : MonoBehaviour {

    [SerializeField]
    Image _picture; // Image à afficher

    [SerializeField]
    Text _name; // Nom à afficher

    [SerializeField]
    PawnType _type; // Type contenu dans l'objet

    Animator anim;

    int selectedPawn; // Pion actuellement séléctionné

    public PawnType type { get { return _type; } }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SelecteNextPawn()
    {
        selectedPawn = selectedPawn + 1 > 4 ? 0 : selectedPawn + 1;

        anim.SetTrigger("Select");

        Refresh();
    }

    public void SelectPawn(int index)
    {
        selectedPawn = Mathf.Clamp(index, 0, 4);
        Refresh();
    }

    void Refresh()
    {
        _type = TeamManager.Instance.availablePawns[selectedPawn];
        _picture.sprite = _type.Picture;
        _name.text = _type.Type;
    }
}
