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
    }

    public void T1PleaseBeExhausted()
    {
        text.text = "하단에 표시되는 체력을 모두 소모할 때까지 달리십시오.\nW키와 왼쪽 Shift키를 동시에 눌러 달릴 수 있습니다.";
    }

    public void T1OpenDoor()
    {
        text.text = "복도 끝의 문을 열고 들어가십시오.\n문 가까이에서 마우스 왼쪽 버튼을 클릭하면 문이 열립니다.";
    }

    public void T1UseKey()
    {
        text.text = "열쇠로 잠긴 출입문을 열고 건물을 탈출하십시오.\n열쇠에 닿으면 열쇠를 획득합니다.";
    }
     
    public void T4FleeFromChaser()
    {
        text.text = "추적자와 거리를 벌리고 추적자에게 보이지 않게 숨으십시오.\n추적자의 눈에 띄거나 근처에서 발소리를 내면 쫓아옵니다.";
    }

    public void T4Escape()
    {
        text.text = "중앙의 계단을 통해 건물을 무사히 탈출하십시오.\n실전에서 추적자를 만나면 누구인지 파악하고 대처하십시오.";
    }

    /*
    public void PleaseRestart()
    {
        text.text = "추적자에게 잡혔습니다.\n\"게임 재시작\" 버튼을 누르십시오.";
    }

    public void SuccessToEscape()
    {
        text.text = "건물을 탈출하는 데 성공했습니다! [Esc키] + \"Quit Game\" 버튼을 누르십시오.";
    }
    */
}
