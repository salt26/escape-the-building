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

    void Awake()
    {
        stxt = this;
    }
	void Start () {
        text = GetComponent<Text>();
#if NEW_VERSION
        if (!(SceneManager.GetActiveScene().name == "1.Terrain and Audio" || SceneManager.GetActiveScene().name == "2.Navigation"))
        {
#endif
            text.text = "당신을 쫓아오는 추적자를 피해 건물 어딘가에 있는 열쇠를 찾고 탈출하십시오.";
            return;
#if NEW_VERSION
        }
        if (SceneManager.GetActiveScene().name != "1.Terrain and Audio") text.text = "움직이는 캡슐을 피해 ";
        text.text += Manager.manager.speakers.Length + "개의 서로 다른 음악을 재생하는 스피커를 찾아가시오.";
#endif
	}

    public void PleaseRestart()
    {
        text.text = "추적자에게 잡혔습니다. \"Restart Game\" 버튼을 누르십시오.";
    }

    public void SuccessToEscape()
    {
        text.text = "건물을 탈출하는 데 성공했습니다! \"Quit Game\" 버튼을 누르십시오.";
    }
}
