using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Manager : MonoBehaviour {

    static public Manager manager;

    public AudioClip fanfare;
    public GameObject[] speakers;
    public GameObject startPanel;
    
    bool[] isSpeakerTouched;
    bool isCleared;
    bool isSoundPlaying;

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
        Time.timeScale = 0f;
        startPanel.SetActive(true);
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
        //DontDestroyOnLoad(GameObject.Find("Directional Light"));
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
        AudioSource BGM = GameObject.Find("Building1").GetComponent<AudioSource>();
        if ((toggle.isOn && BGM.mute) || (!toggle.isOn && !BGM.mute))
        {
            BGM.mute = !toggle.isOn;
        }
    }

    public void GameStart() {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
    }
}
