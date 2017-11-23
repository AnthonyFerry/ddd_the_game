using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TeamPanel : MonoBehaviour {

	void OnEnable()
    {
        TeamManager.Instance.DisplayCurrentTeam();
        Debug.Log("pute");
    }
}
