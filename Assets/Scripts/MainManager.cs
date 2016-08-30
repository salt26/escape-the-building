using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainManager : MonoBehaviour {

    public static MainManager mm;

    public float mouseSensitivity = 2f;
    public bool useHeadBob = true;

    public AudioMixer masterMixer;
    public GameObject mainImage;
    public GameObject mapSelectImage;
    public GameObject optionImage;
    public GameObject loadingUnivImage;
    public GameObject loadingTrainingImage;

    float masterVolume = 0f;
    float BGMVolume = 0f;
    float SFXVolume = 0f;
    bool masterOn = true;
    bool BGMOn = true;
    bool SFXOn = true;

    private static bool multipleMM = false;

    void Awake()
    {
        if (multipleMM) // mm 중복생성이면 기존 mm의 설정을 새 mm으로 옮기고 기존 mm 파괴
        {
            ToggleMaster(mm.GetMasterOn());
            ToggleBGM(mm.GetBGMOn());
            ToggleSFX(mm.GetSFXOn());
            SetHeadBob(mm.GetHeadBob());
            SetMasterVol(mm.GetMasterVol());
            SetBGMVol(mm.GetBGMVol());
            SetSFXVol(mm.GetSFXVol());
            SetSensitivity(mm.GetSensitivity());
            Destroy(mm.gameObject);
        }
        mm = this;
        DontDestroyOnLoad(this);
        LoadMainScene();
    }

    void Start()
    {
        if (!multipleMM) multipleMM = true; // mm 중복생성 방지
        ReturnToMainButton();
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            LoadMainScene();
        }
    }

    public void LoadMainScene()
    {
        mainImage = GameObject.Find("MainImage");
        mapSelectImage = GameObject.Find("MapSelectImage");
        optionImage = GameObject.Find("OptionImage");
        loadingUnivImage = GameObject.Find("LoadingUnivImage");
        loadingTrainingImage = GameObject.Find("LoadingTrainingImage");
    }

    public void SetMasterVol(float masterVol)
    {
        masterVolume = masterVol;
        if(masterOn)
        masterMixer.SetFloat("MasterVolume", masterVolume);
    }

    public void SetBGMVol(float bgmVol)
    {
        BGMVolume = bgmVol;
        if(BGMOn)
        masterMixer.SetFloat("BGMVolume", BGMVolume);
    }

    public void SetSFXVol(float sfxVol)
    {
        SFXVolume = sfxVol;
        if(SFXOn)
        masterMixer.SetFloat("SFXVolume", SFXVolume);
    }

    public void ToggleMaster(bool toggle)
    {
        if (toggle)
        {
            masterOn = true;
            masterMixer.SetFloat("MasterVolume", masterVolume);
        }
        else
        {
            masterOn = false;
            masterMixer.SetFloat("MasterVolume", -80f);
        }
    }

    public void ToggleBGM(bool toggle)
    {
        if (toggle)
        {
            BGMOn = true;
            masterMixer.SetFloat("BGMVolume", BGMVolume);
        }
        else
        {
            BGMOn = false;
            masterMixer.SetFloat("BGMVolume", -80f);
        }
    }

    public void ToggleSFX(bool toggle)
    {
        if (toggle)
        {
            SFXOn = true;
            masterMixer.SetFloat("SFXVolume", SFXVolume);
        }
        else
        {
            SFXOn = false;
            masterMixer.SetFloat("SFXVolume", -80f);
        }
    }

    public void SetSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
        if(Move.move != null)
            Move.move.GetMouseLook().UpdateSensitivity();
    }

    public void SetHeadBob(bool headBob)
    {
        useHeadBob = headBob;
    }

    public void ResetOptions()
    {
        ToggleMaster(true);
        ToggleBGM(true);
        ToggleSFX(true);
        SetHeadBob(true);
        SetMasterVol(0f);
        SetBGMVol(0f);
        SetSFXVol(0f);
        SetSensitivity(2f);
    }

    public bool GetMasterOn()
    {
        return masterOn;
    }

    public bool GetBGMOn()
    {
        return BGMOn;
    }

    public bool GetSFXOn()
    {
        return SFXOn;
    }

    public bool GetHeadBob()
    {
        return useHeadBob;
    }

    public float GetMasterVol()
    {
        return masterVolume;
    }

    public float GetBGMVol()
    {
        return BGMVolume;
    }

    public float GetSFXVol()
    {
        return SFXVolume;
    }

    public float GetSensitivity()
    {
        return mouseSensitivity;
    }

    public void MainStartButton()
    {
        mapSelectImage.SetActive(true);
        mainImage.SetActive(false);
    }

    public void MainOptionButton()
    {
        optionImage.SetActive(true);
        mainImage.SetActive(false);
    }

    public void MainQuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
    }

    public void MapTrainingRoomButton()
    {
        loadingTrainingImage.SetActive(true);
        mapSelectImage.SetActive(false);
        SceneManager.LoadScene("TrainingRoom1");
    }

    public void MapUniversityButton()
    {
        loadingUnivImage.SetActive(true);
        mapSelectImage.SetActive(false);
        SceneManager.LoadScene("5.Building");
    }

    public void ReturnToMainButton()
    {
        mainImage.SetActive(true);
        optionImage.SetActive(false);
        mapSelectImage.SetActive(false);
        loadingTrainingImage.SetActive(false);
        loadingUnivImage.SetActive(false);
    }
}
