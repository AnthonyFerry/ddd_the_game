using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Management : SwissArmyKnife.Singleton<UI_Management> {

    GameManager _manager;

    public GameObject _playerPanel;
    public GameObject _enemyPanel;
    public GameObject _iconReference;

    public Color _playerColor;
    public Color _enemyColor;
    public Color _testColor;

    public List<UI_Pawn_Icon> _iconsPlayer = new List<UI_Pawn_Icon>();
    public List<UI_Pawn_Icon> _iconsEnemy = new List<UI_Pawn_Icon>();

    public void Init(GameManager man) {
        _manager = man;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UI_Pawn_Icon createIcon(BasePawn parent, bool owner)
    {
        UI_Pawn_Icon nv;
        GameObject no;
        
        if (owner){
            no = Instantiate(_iconReference, _playerPanel.transform);
            no.transform.localPosition = new Vector3(0, 350 - _iconsPlayer.Count * 175, 0);
            no.name = "Icon_" + parent.gameObject.name;
            nv = no.AddComponent<UI_Pawn_Icon>();
            nv._parent = parent;
            nv._isPlayer = owner;
            nv._maxLife = nv._life = parent.health;
            nv._color = _playerColor;
            //nv._color = _testColor;
            nv.Init();
            _iconsPlayer.Add(nv);
        } else {
            no = Instantiate(_iconReference, _enemyPanel.transform);
            no.transform.localPosition = new Vector3(0, 350 - _iconsEnemy.Count * 175, 0);
            no.name = "Icon_" + parent.gameObject.name;
            nv = no.AddComponent<UI_Pawn_Icon>();
            nv._parent = parent;
            nv._isPlayer = owner;
            nv._maxLife = nv._life = parent.health;
            nv._color = _enemyColor;
            nv.Init();
            _iconsEnemy.Add(nv);
        }

        return nv;
    }

}