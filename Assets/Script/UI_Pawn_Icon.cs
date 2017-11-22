using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[SerializeField]
public class UI_Pawn_Icon : MonoBehaviour {

    public BasePawn _parent;
    public bool _isPlayer;
    public Color _color;
    public float _life;
    public float _maxLife;

    Image _image;
    Image _icon;
    public bool _calculateDamages = false;
    
    public void Init (PawnType type) {
        _icon = GetChildGameObject(GetChildGameObject(this.gameObject, "Background"), "Center").GetComponent<Image>();
        _image = GetChildGameObject(GetChildGameObject(this.gameObject, "Background"), "LoadingBar").GetComponent<Image>();
        if (_image != null) Debug.Log("Image for "+this.gameObject.name+" setted");
        _image.color = _color;
        _image.fillAmount = _life / _maxLife;
        _icon.sprite = type.Picture; 
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float Phil = _image.fillAmount;
        float ratioLife = _life / _maxLife;

        if (_image.fillAmount <= ratioLife)
        {
            _calculateDamages = false;
        }

        if (_calculateDamages)
        {
            _image.fillAmount = Mathf.MoveTowards(Phil, ratioLife, 0.05f);
        }

    }

    public void updateDamages(float actualLife)
    {
        updateDamages((int) actualLife);
    }

    public void updateDamages(int actualLife) {
        _life = actualLife;
        _calculateDamages = true;
        //_image.fillAmount = _life / _maxLife;
    }

    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

}
