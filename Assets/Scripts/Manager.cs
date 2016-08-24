using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Manager : MonoBehaviour {

    // 게임 오버 또는 탈출 여부를 기록함.
    static public Manager manager;

    public GameObject startPanel;
    public GameObject[] chasers;    // 여기 등록된 추적자만 공식적으로 사용 가능
    public AudioMixer BGMMixer;

    private Animator gameOverAnim;
    bool isStart;
    bool hasEscaped;
    bool isGameOver;

    void Awake()
    {
        manager = this;
    }

    void Start()
    {
        GetComponent<Move>().GetMouseLook().SetCursorLock(false);
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        isStart = false;
        hasEscaped = false;
        isGameOver = false;
        gameOverAnim = GameObject.Find("GameOverPanel").GetComponent<Animator>();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
    }

    public void BGMOn()
    {
        Toggle toggle = GameObject.Find("Toggle").GetComponent<Toggle>();
        if (toggle.isOn)
        {
            BGMMixer.SetFloat("BGM", 0f);
        }
        else
        {
            BGMMixer.SetFloat("BGM", -80f);
        }
    }

    public void GameStart() {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        GetComponent<Move>().GetMouseLook().SetCursorLock(true);
        isStart = true;
    }

    public bool IsStart()
    {
        return isStart;
    }

    public void SetHasEscaped()
    {
        StateText.stxt.SuccessToEscape();
        NoticeText.ntxt.NoticeEscaped();
        gameOverAnim.SetTrigger("successToEscape");
        hasEscaped = true;
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
}
