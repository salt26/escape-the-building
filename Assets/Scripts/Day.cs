using UnityEngine;
using System.Collections;

public class Day : MonoBehaviour {

    Light dirLight;
    Color day;
	// Use this for initialization
	void Start () {
        dirLight = GetComponent<Light>();
        day = dirLight.color;
	}
	
	// Update is called once per frame
	void Update () {
        if ((int)((Time.time + 50f) / 30f) % 2 == 0)
            dirLight.color = Color.Lerp(dirLight.color, Color.black, Time.deltaTime*0.1f);
        else dirLight.color = Color.Lerp(dirLight.color, day, Time.deltaTime * 0.1f);	
	}
}
