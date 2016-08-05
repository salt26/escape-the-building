using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour {

    // 게임 오버 또는 탈출 여부를 기록하는 클래스

    static public Escape escape;

    private Animator gameOverAnim;
    bool isGameOver;
    bool hasEscaped;

    void Awake()
    {
        escape = this;
    }

    void Start()
    {
        hasEscaped = false;
        isGameOver = false;
        gameOverAnim = GameObject.Find("GameOverPanel").GetComponent<Animator>();
    }

    public bool GetHasEscaped()
    {
        return hasEscaped;
    }

    public void SetIsGameOver()
    {
        isGameOver = true;
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player" && !hasEscaped)
        {
            StateText.stxt.SuccessToEscape();
            NoticeText.ntxt.NoticeEscaped();
            gameOverAnim.SetTrigger("successToEscape");
            hasEscaped = true;
        }
    }
}
