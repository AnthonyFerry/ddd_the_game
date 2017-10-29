using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[SerializeField]
public class UI_Pawn_Icon : MonoBehaviour {

    public BasePawn _parent;
    public bool _isPlayer;
    public Color _color;
    public int _life;
    public int _maxLife;

    Image _image;
    
    public void Init () {
        _image = GetChildGameObject(GetChildGameObject(this.gameObject, "Background"), "LoadingBar").GetComponent<Image>();
        if (_image != null) Debug.Log("Image for "+this.gameObject.name+" setted");
        _image.color = _color;
        _image.fillAmount = _life / _maxLife;
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateDamages(int actualLife) {
        _life = actualLife;
        _image.fillAmount = _life / _maxLife;
    }

    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

}
