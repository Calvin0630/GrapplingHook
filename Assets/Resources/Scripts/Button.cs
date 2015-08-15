using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public void LoadMultiplayer() {
		Application.LoadLevel ("FirstLevel");
	}

    public void LoadSinglePlayer() {
        Application.LoadLevel("SinglePlayer");
    }

}
