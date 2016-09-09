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
    public GameObject menuPanel;
    public GameObject helpPanel;
    public GameObject optionPanel;
    public GameObject helpObjective;
    public GameObject helpControl;
    public GameObject helpChaser;
    public GameObject helpCredit;
    public GameObject helpPanel_;       // _가 붙은 변수와 함수들은 시작 시 띄워주는 도움말과 관계가 있음.
    public GameObject helpObjective_;
    public GameObject helpControl_;
    public GameObject helpChaser_;
    public GameObject helpCredit_;
    public GameObject[] chasers;        // 여기 등록된 추적자만 공식적으로 사용 가능
    public AudioMixer BGMMixer;
    public AudioSource buildings;
    public AudioClip escapeBGM;
    public GameObject escapeBox;

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

    void Update()
    {
        if (isStart && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuButton();
        }
        if (isGameOver)
        {
            Time.timeScale = 1f;
            GetComponent<Move>().GetMouseLook().SetCursorLock(false);
        }
    }

    public void MenuButton()
    {
        if (isStart && !menuPanel.activeInHierarchy)
        {
            menuPanel.SetActive(true);
            helpPanel.SetActive(false);
            optionPanel.SetActive(false);
            Time.timeScale = 0f;
            GetComponent<Move>().GetMouseLook().SetCursorLock(false);
        }
        else if (isStart)
        {
            menuPanel.SetActive(false);
            helpPanel.SetActive(false);
            optionPanel.SetActive(false);
            Time.timeScale = 1f;
            GetComponent<Move>().GetMouseLook().SetCursorLock(true);
        }

    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        helpPanel.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 1f;
        GetComponent<Move>().GetMouseLook().SetCursorLock(true);
    }

    public void HelpButton()
    {
        helpPanel.SetActive(true);
        menuPanel.SetActive(false);
        HelpObjective();
    }

    public void HelpButton_()
    {
        helpPanel_.SetActive(true);
        startPanel.SetActive(false);
        HelpObjective_();
    }

    public void OptionButton()
    {
        optionPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void HelpObjective()
    {
        helpObjective.SetActive(true);
        helpControl.SetActive(false);
        helpChaser.SetActive(false);
        helpCredit.SetActive(false);
    }

    public void HelpControl()
    {
        helpObjective.SetActive(false);
        helpControl.SetActive(true);
        helpChaser.SetActive(false);
        helpCredit.SetActive(false);
    }

    public void HelpChaser()
    {
        helpObjective.SetActive(false);
        helpControl.SetActive(false);
        helpChaser.SetActive(true);
        helpCredit.SetActive(false);
    }

    public void HelpCredit()
    {
        helpObjective.SetActive(false);
        helpControl.SetActive(false);
        helpChaser.SetActive(false);
        helpCredit.SetActive(true);
    }

    public void HelpObjective_()
    {
        helpObjective_.SetActive(true);
        helpControl_.SetActive(false);
        helpChaser_.SetActive(false);
        helpCredit_.SetActive(false);
    }

    public void HelpControl_()
    {
        helpObjective_.SetActive(false);
        helpControl_.SetActive(true);
        helpChaser_.SetActive(false);
        helpCredit_.SetActive(false);
    }

    public void HelpChaser_()
    {
        helpObjective_.SetActive(false);
        helpControl_.SetActive(false);
        helpChaser_.SetActive(true);
        helpCredit_.SetActive(false);
    }

    public void HelpCredit_()
    {
        helpObjective_.SetActive(false);
        helpControl_.SetActive(false);
        helpChaser_.SetActive(false);
        helpCredit_.SetActive(true);
    }

    public void SetMasterVol(float masterVol)
    {
        MainManager.mm.SetMasterVol(masterVol);
    }

    public void SetBGMVol(float bgmVol)
    {
        MainManager.mm.SetBGMVol(bgmVol);
    }

    public void SetSFXVol(float sfxVol)
    {
        MainManager.mm.SetSFXVol(sfxVol);
    }

    public void ToggleMaster(bool toggle)
    {
        MainManager.mm.ToggleMaster(toggle);
    }

    public void ToggleBGM(bool toggle)
    {
        MainManager.mm.ToggleBGM(toggle);
    }

    public void ToggleSFX(bool toggle)
    {
        MainManager.mm.ToggleSFX(toggle);
    }

    public void SetSensitivity(float sensitivity)
    {
        MainManager.mm.SetSensitivity(sensitivity);
    }

    public void SetHeadBob(bool headBob)
    {
        MainManager.mm.SetHeadBob(headBob);
    }

    public void ResetOptions()
    {
        MainManager.mm.ResetOptions();
    }

    public void GameStart() {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        helpPanel_.SetActive(false);
        GetComponent<Move>().GetMouseLook().SetCursorLock(true);
        isStart = true;
    }

    public void OpenMsgBox()
    {
        isStart = false;
        Time.timeScale = 0f;
        GetComponent<Move>().GetMouseLook().SetCursorLock(false);
    }

    public void CloseMsgBox()
    {
        isStart = true;
        Time.timeScale = 1f;
        GetComponent<Move>().GetMouseLook().SetCursorLock(true);
    }

    public bool GetIsStart()
    {
        return isStart;
    }

    public void SetIsStart(bool start)
    {
        isStart = start;
    }

    public void SetHasEscaped()
    {
        NoticeText.ntxt.NoticeEscaped();
        gameOverAnim.SetTrigger("successToEscape");
        hasEscaped = true;
        if (SceneManager.GetActiveScene().name == "5.Building")
        {
            buildings.clip = escapeBGM;
            buildings.Play();
            StartCoroutine("EscapeUniversity");
        }
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

    public void LoadTutorial2()
    {
        SceneManager.LoadScene("TrainingRoom2");
    }

    public void LoadTutorial3()
    {
        SceneManager.LoadScene("TrainingRoom3");
    }

    public void LoadTutorial4()
    {
        SceneManager.LoadScene("TrainingRoom4");
    }

    IEnumerator EscapeUniversity()
    {
        yield return new WaitForSeconds(3f);
        CloseMenu();
        escapeBox.SetActive(true);
        OpenMsgBox();
    }
}
