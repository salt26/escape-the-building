using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionController : MonoBehaviour {

    public Slider masterVolSlider;
    public Slider BGMVolSlider;
    public Slider SFXVolSlider;
    public Slider mouseSensSlider;

    public Toggle masterToggle;
    public Toggle BGMToggle;
    public Toggle SFXToggle;
    public Toggle headBobToggle;

    void Start()
    {
        masterVolSlider.value = MainManager.mm.GetMasterVol();
        BGMVolSlider.value = MainManager.mm.GetBGMVol();
        SFXVolSlider.value = MainManager.mm.GetSFXVol();
        mouseSensSlider.value = MainManager.mm.GetSensitivity();
        masterToggle.isOn = MainManager.mm.GetMasterOn();
        BGMToggle.isOn = MainManager.mm.GetBGMOn();
        SFXToggle.isOn = MainManager.mm.GetSFXOn();
        headBobToggle.isOn = MainManager.mm.GetHeadBob();
    }

    public void ResetOptions()
    {
        masterVolSlider.value = 0f;
        BGMVolSlider.value = 0f;
        SFXVolSlider.value = 0f;
        mouseSensSlider.value = 2f;
        masterToggle.isOn = true;
        BGMToggle.isOn = true;
        SFXToggle.isOn = true;
        headBobToggle.isOn = true;
        MainManager.mm.ResetOptions();
    }
}
