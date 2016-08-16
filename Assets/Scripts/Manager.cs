#define NEW_VERSION
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Linq;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Manager : MonoBehaviour {

    static public Manager manager;

    public AudioClip fanfare;
    public GameObject[] speakers;
    public GameObject startPanel;
    public GameObject[] chasers;    // 여기 등록된 추적자만 공식적으로 사용 가능
    public AudioMixer BGMMixer;
    
    bool[] isSpeakerTouched;
    bool isCleared;
    bool isSoundPlaying;
    bool isStart;

    AudioSource sound;

    void Awake()
    {
        manager = this;
        sound = GetComponent<AudioSource>();
        sound.clip = fanfare;
    }

    void Start()
    {
        isSpeakerTouched = Enumerable.Repeat(false, speakers.Length).ToArray();
        isCleared = false;
        GetComponent<Move>().GetMouseLook().SetCursorLock(false);
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        isStart = false;
    }

	void FixedUpdate () {
        if (!isCleared) CheckIfTouched();
        if (!isSoundPlaying)
        {
            for (int i = 0; i < speakers.Length; i++)
            {
                if (Vector3.Distance(GetComponent<Transform>().position, speakers[i].GetComponent<Transform>().position) > speakers[i].GetComponent<AudioSource>().maxDistance
                    && speakers[i].GetComponent<AudioSource>().isPlaying)
                    speakers[i].GetComponent<AudioSource>().Stop();
                else if (!speakers[i].GetComponent<AudioSource>().isPlaying) speakers[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    void CheckIfTouched()
    {
        bool isAllTouched = true;
        for (int i = 0; i < speakers.Length; i++)
        {
            if (!isSpeakerTouched[i] && Vector3.Distance(GetComponent<Transform>().position, speakers[i].GetComponent<Transform>().position) <= 15f)
            {
                isSpeakerTouched[i] = true;
                speakers[i].GetComponent<AudioSource>().priority = Mathf.Min(129 + i, 255);
                StartCoroutine("PlaySound");
            }
            if (!isSpeakerTouched[i]) isAllTouched = false;
        }
        if (isAllTouched)
        {
            isCleared = true;
        }
    }

    IEnumerator PlaySound()
    {
        isSoundPlaying = true;
        sound.priority = 0;
        sound.Play();
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].GetComponent<AudioSource>().Pause();
        }
        yield return new WaitForSeconds(sound.clip.length);
        sound.priority = 128;
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].GetComponent<AudioSource>().UnPause();
        }
        isSoundPlaying = false;
    }

    /// <summary>
    /// isSpeakerTouched[i] 값을 반환합니다.
    /// </summary>
    public bool IsSpeakerTouched(int i) {
        if (i < 0) return false;
        return isSpeakerTouched[i];
    }

    public void RestartGame()
    {
#if NEW_VERSION
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#else
		Application.LoadLevel ("4.Asset");
#endif
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
        /*
        AudioSource BGM = GameObject.Find("Building1").GetComponent<AudioSource>();
        if ((toggle.isOn && BGM.mute) || (!toggle.isOn && !BGM.mute))
        {
            BGM.mute = !toggle.isOn;
        }
        */
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
}
