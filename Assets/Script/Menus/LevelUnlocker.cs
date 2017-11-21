using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour {

    public Text MessageText;
    public InputField PlayerInput;

    Color messageColor = Color.green;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckPassword()
    {
        string msg = "";
        var world = MenuDatas.Instance.GetWorldByPassword(PlayerInput.text);
        if (world == null)
        {
            msg = "Le mot de passe est incorrect...";
            messageColor = Color.red;
            StartCoroutine("DisplayMessage", msg);
            return;
        }
        
        msg = "Le monde " + world.name + " à été débloqué.";
        messageColor = Color.green;
        StartCoroutine("DisplayMessage", msg);

        SaveManager.SaveProgression();

        PlayerInput.text = "";
    }

    IEnumerator DisplayMessage(string message)
    {
        MessageText.color = messageColor;
        MessageText.text = message;

        yield return new WaitForSeconds(3f);

        MessageText.text = "";
    }

    public void ClosePanel()
    {
        PlayerInput.text = "";
        gameObject.SetActive(false);
    }
}
