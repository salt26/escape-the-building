using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoticeText : MonoBehaviour {

    static public NoticeText ntxt;

    int whichCoroutineStarts;
    int whichCoroutineWorks;

    Text text;

    void Awake()
    {
        ntxt = this;
    }

	void Start () {

        text = GetComponent<Text>();

        whichCoroutineStarts = 0;
        whichCoroutineWorks = 0;
        text.text = "";

	}
	
	void FixedUpdate () {
        
        if (whichCoroutineStarts == 1 && whichCoroutineWorks != 1)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(1, "문이 잠긴 듯하다."));
        }
        else if (whichCoroutineStarts == 2 && whichCoroutineWorks != 2)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(2, "이 문은 가지고 있는 열쇠들로 열리지 않는다."));
        }
        else if (whichCoroutineStarts == 3 && whichCoroutineWorks != 3)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(3, "어, 가지고 있던 열쇠로 문이 열렸다!"));
        }
        else if (whichCoroutineStarts == 4 && whichCoroutineWorks != 4)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(4, "어떤 방 문의 열쇠를 주웠다."));
        }
        else if (whichCoroutineStarts == 5 && whichCoroutineWorks != 5)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(5, "드디어 출입문을 여는 열쇠를 찾았다!"));
        }
        else if (whichCoroutineStarts == 6 && whichCoroutineWorks != 6)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(6, "으아아아아아아아아아악!"));
        }
        else if (whichCoroutineStarts == 7 && whichCoroutineWorks != 7)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(7, "밖이 보인다! 탈출이다!"));
        }
	}

    public void NoticeLockedDoor()
    {
        whichCoroutineStarts = 1;
    }

    public void NoticeFailedToUnlock()
    {
        whichCoroutineStarts = 2;
    }

    public void NoticeSuccessedToUnlock()
    {
        whichCoroutineStarts = 3;
    }

    public void NoticePickRoomKeyUp()
    {
        whichCoroutineStarts = 4;
    }

    public void NoticePickEntranceKeyUp()
    {
        whichCoroutineStarts = 5;
    }

    public void NoticeCaptured()
    {
        whichCoroutineStarts = 6;
    }

    public void NoticeEscaped()
    {
        whichCoroutineStarts = 7;
    }
    IEnumerator NoticeMessage(int coroutineNumber, string message)
    {
        whichCoroutineStarts = 0;
        whichCoroutineWorks = coroutineNumber;
        text.color = Color.white;
        text.text = message;
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 200; i++)
        {
            text.color = Color.Lerp(text.color, Color.clear, Time.fixedDeltaTime * 2f);
            yield return new WaitForSeconds(0.01f);
        }
        text.text = "";
        whichCoroutineStarts = 0;
        whichCoroutineWorks = 0;
    }
}
