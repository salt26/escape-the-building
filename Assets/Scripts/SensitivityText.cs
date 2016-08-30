using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SensitivityText : MonoBehaviour {

    Text sensTxt;

    void Start()
    {
        sensTxt = GetComponent<Text>();
    }
	// Update is called once per frame
	void Update () {
        sensTxt.text = "" + (float)((int)(MainManager.mm.mouseSensitivity * 100f)) / 100f;
	}
}
