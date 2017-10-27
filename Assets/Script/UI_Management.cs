using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Management : MonoBehaviour {

    GameManager _manager;

    public GameObject _playerPanel;
    public GameObject _enemyPanel;
    public GameObject _iconReference;

    public List<UI_Pawn_Icon> _iconsPlayer = new List<UI_Pawn_Icon>();
    public List<UI_Pawn_Icon> _iconsEnemy = new List<UI_Pawn_Icon>();

    public void Init(GameManager man) {
        _manager = man;
        foreach (BasePawn bp in _manager.Pawns) {
            if (bp.isPlayer)
            {
                _iconsPlayer.Add(createIcon(bp, true));
            }
            else
            {
                _iconsEnemy.Add(createIcon(bp, false));
            }
        }

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    UI_Pawn_Icon createIcon(BasePawn parent, bool owner)
    {
        UI_Pawn_Icon nv;
        GameObject no;
        
        if (owner){
            no = Instantiate(_iconReference, _playerPanel.transform);
            no.transform.localPosition = new Vector3(0, 350 - _iconsPlayer.Count * 175, 0);
            no.name = "Icon_" + parent.gameObject.name;
            nv = no.AddComponent<UI_Pawn_Icon>();
            nv._color = new Color(12, 212, 195, 255);
        } else {
            no = Instantiate(_iconReference, _enemyPanel.transform);
            no.transform.localPosition = new Vector3(0, 350 - _iconsEnemy.Count * 175, 0);
            no.name = "Icon_" + parent.gameObject.name;
            nv = no.AddComponent<UI_Pawn_Icon>();
            nv._color = new Color(203, 19, 19, 255);
        }
        nv._parent = parent;
        nv._isPlayer = owner;
        nv._maxLife = nv._life = parent.health;

        return nv;
    }

}

[SerializeField]
public struct Icons {
    public GameObject _object;
    public BasePawn _parent;
    public bool _isPlayer;
    public Color _color;
    public int _life;
    public int _maxLife;
}