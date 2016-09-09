using UnityEngine;
using System.Collections;

public class TutorialLocation : MonoBehaviour {


    int locationID;
    bool[] showBoxOnce = new bool[8];
    public GameObject[] msgBoxes;

    void Start()
    {
        locationID = GetComponent<Location>().GetLocationID();
        for(int i = 0; i < 8; i++)
        {
            showBoxOnce[i] = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Manager.manager.GetIsStart())
        {
            switch (locationID)
            {
                case 1101:
                    if (showBoxOnce[0]) break;
                    showBoxOnce[0] = true;
                    Move.move.SetStamina(4f);
                    msgBoxes[0].SetActive(true);
                    StateText.stxt.T1PleaseBeExhausted();
                    Manager.manager.OpenMsgBox();
                    break;
                case 1102:
                    if (showBoxOnce[1]) break;
                    else if (!Move.move.GetTutorialBox_()) break;
                    else if (Move.move.isExhausted) break;
                    showBoxOnce[1] = true;
                    msgBoxes[1].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 1103:
                    if (showBoxOnce[2]) break;
                    showBoxOnce[2] = true;
                    msgBoxes[2].SetActive(true);
                    StateText.stxt.T1UseKey();
                    Manager.manager.OpenMsgBox();
                    break;
                case 1104:
                    if (showBoxOnce[3]) break;
                    showBoxOnce[3] = true;
                    StartCoroutine("Tutorial1Clear");
                    break;
                case 2202:
                    if (showBoxOnce[4]) break;
                    showBoxOnce[4] = true;
                    msgBoxes[0].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 2201:
                    if (showBoxOnce[5]) break;
                    showBoxOnce[5] = true;
                    StartCoroutine("Tutorial2Clear");
                    break;
                case 3102:
                    if (showBoxOnce[6]) break;
                    showBoxOnce[6] = true;
                    StartCoroutine("Tutorial3Clear");
                    break;
                case 4121:
                    if (showBoxOnce[7]) break;
                    showBoxOnce[7] = true;
                    StartCoroutine("Tutorial4Clear");
                    break;

            }
        }
    }

    IEnumerator Tutorial1Clear()
    {
        yield return new WaitForSeconds(3f);
        Manager.manager.CloseMenu();
        msgBoxes[3].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial2Clear()
    {
        yield return new WaitForSeconds(3f);
        Manager.manager.CloseMenu();
        msgBoxes[1].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial3Clear()
    {
        yield return new WaitForSeconds(3f);
        Manager.manager.CloseMenu();
        msgBoxes[0].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial4Clear()
    {
        yield return new WaitForSeconds(3f);
        Manager.manager.CloseMenu();
        msgBoxes[0].SetActive(true);
        Manager.manager.OpenMsgBox();
    }
}
