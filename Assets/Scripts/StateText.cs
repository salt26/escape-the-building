using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StateText : MonoBehaviour {

    public static StateText stxt;
    Text text;

    void Awake()
    {
        stxt = this;
    }
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "당신을 쫓아오는 추적자를 피해 건물 어딘가에 있는 열쇠를 찾고 탈출하십시오.";
        return;
    }

    public void PleaseRestart()
    {
        text.text = "추적자에게 잡혔습니다. \"Restart Game\" 버튼을 누르십시오.";
    }

    public void SuccessToEscape()
    {
        text.text = "건물을 탈출하는 데 성공했습니다! [Esc키] + \"Quit Game\" 버튼을 누르십시오.";
    }
}
