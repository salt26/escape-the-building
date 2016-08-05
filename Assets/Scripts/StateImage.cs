using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StateImage : MonoBehaviour {

    int speakerID;
    bool isCleared;

    void Start()
    {
        isCleared = false;
        GetComponent<Image>().color = new Color(208f / 255f, 222f / 255f, 238f / 255f, 1f);
        //speakerID = -1;
    }

    public void SetSpeaker(int ID)
    {
        speakerID = ID;
    }

    public void Clear()
    {
        if (speakerID < 0 || isCleared) return;
        isCleared = true;
        GetComponent<Image>().color = new Color(58f / 255f, 62f / 255f, 71f / 255f, 1f);
        
    }

}
