using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TeamPanel : MonoBehaviour {

	void OnEnable()
    {
        TeamManager.Instance.panel = this.gameObject;

        TeamManager.Instance.DisplayCurrentTeam();
    }

    public void ValidateTeam()
    {
        TeamManager.Instance.ValidateTeam();
    }
}
