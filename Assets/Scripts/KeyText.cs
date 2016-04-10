#define NEW_VERSION
using UnityEngine;
using UnityEngine.UI;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif
using System.Collections;

public class KeyText : MonoBehaviour
{

    public static KeyText ktxt;
    int initialKeyNumber; // 처음 건물에 뿌려진 열쇠 수
    Text text;

    void Start()
    {
        ktxt = this;
        text = GetComponent<Text>();
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "3.Modeling")
        {
#endif
            text.text = "열쇠를 하나라도 찾으면 열쇠에 맞는 문을 찾아 열고 나가십시오.           0 / 2";
            return;
#if NEW_VERSION
        }
#endif
    }

    void FixedUpdate()
    {
        text.text = "열쇠를 하나라도 찾으면 열쇠에 맞는 문을 찾아 열고 나가십시오.";
        text.text += "           " + BuildingManager.buildingManager.NumberOfNotUsedKey();
        text.text += " / " + (initialKeyNumber - BuildingManager.buildingManager.NumberOfUsedKey());

    }

    public void SetInitialKeyNumber(int keyNumber)
    {
        initialKeyNumber = keyNumber;
    }
}
