using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyText : MonoBehaviour
{

    public static KeyText ktxt;

    int initialKeyNumber; // 처음 건물에 뿌려진 열쇠 수
    Text text;
    KeyInventory inventory;

    void Awake()
    {
        ktxt = this;
    }

    void Start()
    {
        text = GetComponent<Text>();
        inventory = GameObject.Find("Player").GetComponent<KeyInventory>();

            text.text = "0 / 7";
            return;
    }

    void FixedUpdate()
    {
        if (!Manager.manager.GetHasEscaped() && !Manager.manager.GetIsGameOver())
        {
            text.text = "" + inventory.NumberOfNotUsedKey();
            text.text += " / " + (initialKeyNumber - inventory.NumberOfUsedKey());
        }
        else
        {
            text.text = "" + inventory.NumberOfNotUsedKey();
            text.text += " / " + (initialKeyNumber - inventory.NumberOfUsedKey());
        }

    }

    public void SetInitialKeyNumber(int keyNumber)
    {
        initialKeyNumber = keyNumber;
    }
}
