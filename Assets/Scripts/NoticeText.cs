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
            int r = Random.Range(0, 2);
            if (r == 0) StartCoroutine(NoticeMessage(6, "으아아아아아아아아아악!"));
            else if (r == 1) StartCoroutine(NoticeMessage(6, "그들에게 붙잡히고 말았다..."));
        }
        else if (whichCoroutineStarts == 7 && whichCoroutineWorks != 7)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(7, "밖이 보인다! 탈출이다!"));
        }
        else if (whichCoroutineStarts == 8 && whichCoroutineWorks != 8)
        {
            StopAllCoroutines();
            int r = Random.Range(0, 2);
            if (r == 0) StartCoroutine(NoticeMessage(8, "힘들어 죽을 것 같아."));
            else if (r == 1) StartCoroutine(NoticeMessage(8, "이대로 쓰러질 수는 없어..."));
        }
        else if (whichCoroutineStarts == 9 && whichCoroutineWorks != 9)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(9, "추적자가 탈진했다. 어서 도망가자!"));
        }
        else if (whichCoroutineStarts == 10 && whichCoroutineWorks != 10)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(10, "쉿, 가까이에 추적자가 있다."));
        }
        else if (whichCoroutineStarts == 11 && whichCoroutineWorks != 11)
        {
            StopAllCoroutines();
            int r = Random.Range(0, 2);
            if (r == 0) StartCoroutine(NoticeMessage(11, "추적자가 바로 앞까지 쫓아왔어!"));
            else if (r == 1) StartCoroutine(NoticeMessage(11, "앗, 위험하다!"));
        }
        else if (whichCoroutineStarts == 12 && whichCoroutineWorks != 12)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(12, "밖으로 통하는 문이지만, 잠긴 듯하다."));
        }
        else if (whichCoroutineStarts == 13 && whichCoroutineWorks != 13)
        {
            StopAllCoroutines();
            StartCoroutine(NoticeMessage(13, "이 출입문은 가지고 있는 열쇠들로 열리지 않는다."));
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

    public void NoticePlayerExhausted()
    {
        whichCoroutineStarts = 8;
    }

    public void NoticeChaserExhausted()
    {
        whichCoroutineStarts = 9;
    }

    public void NoticeChaserApproachByWalking()
    {
        whichCoroutineStarts = 10;
    }

    public void NoticeChaserApproachByRunning()
    {
        whichCoroutineStarts = 11;
    }

    public void NoticeLockedEntranceDoor()
    {
        whichCoroutineStarts = 12;
    }

    public void NoticeFailedToUnlockEntrance()
    {
        whichCoroutineStarts = 13;
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
