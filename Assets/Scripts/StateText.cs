#define NEW_VERSION
using UnityEngine;
using UnityEngine.UI;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif
using System.Collections;

public class StateText : MonoBehaviour {

    public static StateText stxt;
    Text text;
	
	void Start () {
        stxt = this;
        text = GetComponent<Text>();
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "3.Modeling")
        {
#endif
            text.text = "당신을 쫓아오는 캡슐을 피해 건물 어딘가에 있는 열쇠를 찾고 탈출하시오.";
            return;
#if NEW_VERSION
        }
        if (SceneManager.GetActiveScene().name != "1.Terrain and Audio") text.text = "움직이는 캡슐을 피해 ";
        text.text += Manager.manager.speakers.Length + "개의 서로 다른 음악을 재생하는 스피커를 찾아가시오.";
#endif
	}

    public void PleaseRestart()
    {
        text.text = "캡슐에 부딪혀 움직일 수 없게 되었습니다. \"Restart Game\" 버튼을 누르시오.";
    }
}
