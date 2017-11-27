using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Management : SwissArmyKnife.Singleton<UI_Management> {

    GameManager _manager;

    [Header("Everything to manage pawns' icons")]

    public GameObject _playerPanel;
    public GameObject _enemyPanel;
    public GameObject _iconReference;

    public Color _playerColor;
    public Color _enemyColor;

    public List<UI_Pawn_Icon> _iconsPlayer = new List<UI_Pawn_Icon>();
    public List<UI_Pawn_Icon> _iconsEnemy = new List<UI_Pawn_Icon>();

    [Header("And this is about the in-game texts")]

    public Text _gameText;
    public Animator _textAnim;
    float _tempus;

    [Header("End Game panel(s)")]

    public GameObject _endPanel;
    public Text _endText;
    public GameObject[] _endButtons; // 0 is backmenu, 1 is retry, 2 is continue.
    public Animator _endAnim;

    public void Init(GameManager man) {
        _manager = man;
    }

	// Use this for initialization
	void Start () {
		
	}
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

    }

	void FixedUpdate () {
        if (_manager.Locker == GameState.locked)
        {
            _tempus += Time.deltaTime;

            if(_tempus > 2)
            {
                Debug.Log("Unlocked !");
                _tempus = 0;
                _manager.Locker = GameState.unlocked;
            }
        }
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
            nv.Init(parent.type);
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
            nv.Init(parent.type);
            _iconsEnemy.Add(nv);
        }

        return nv;
    }

    public void changingTurn(bool isPlayer) {
        if (isPlayer)
        {
            _gameText.text = "Tour du Joueur";
            _textAnim.SetTrigger("TriggerShow");
        }
        else
        {
            _gameText.text = "Tour de l'adversaire";
            _textAnim.SetTrigger("TriggerShow");
        }
    }

    public void callEndGame(bool isVictory) {
        if (isVictory) {
            _endText.text = "Victoire !";
            _endButtons[0].transform.localPosition = new Vector3(-300, -120);
            _endButtons[1].transform.localPosition = new Vector3(0, -120);
            _endButtons[2].SetActive(true);
            _endAnim.SetTrigger("TriggerVictory");
        } else {
            _endText.text = "Défaite...";
            _endButtons[0].transform.localPosition = new Vector3(-150, -120);
            _endButtons[1].transform.localPosition = new Vector3(150, -120);
            _endButtons[2].SetActive(false);
            _endAnim.SetTrigger("TriggerDefeat");
        }
    }

    public void OC_Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void OC_BackMenu()
    {
        SceneManager.LoadScene(0);
        TeamManager.Instance.DisplayCurrentTeam();
    }

    public void OC_Continue()
    {
        Level nextLevel = null;

        if ((nextLevel = MenuDatas.Instance.UnlockNextLevel()) == null)
            nextLevel = MenuDatas.Instance.GetNextLevel();
                

        if (nextLevel == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            MenuDatas.Instance.selectedLevel = nextLevel;
            SceneManager.LoadScene(1);
        }
    }

}