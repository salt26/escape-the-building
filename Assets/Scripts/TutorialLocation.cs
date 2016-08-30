using UnityEngine;
using System.Collections;

public class TutorialLocation : MonoBehaviour {


    int locationID;
    public GameObject[] msgBoxes;

    void Start()
    {
        locationID = GetComponent<Location>().GetLocationID();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (locationID)
            {
                case 1101:
                    Move.move.SetStamina(4f);
                    msgBoxes[0].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 1102:
                    msgBoxes[1].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 1103:
                    msgBoxes[2].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 1104:
                    StartCoroutine("Tutorial1Clear");
                    break;
                case 2202:
                    msgBoxes[0].SetActive(true);
                    Manager.manager.OpenMsgBox();
                    break;
                case 2201:
                    StartCoroutine("Tutorial2Clear");
                    break;
                case 3102:
                    StartCoroutine("Tutorial3Clear");
                    break;
                case 4121:
                    StartCoroutine("Tutorial4Clear");
                    break;

            }
        }
    }

    IEnumerator Tutorial1Clear()
    {
        yield return new WaitForSeconds(3f);
        msgBoxes[3].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial2Clear()
    {
        yield return new WaitForSeconds(3f);
        msgBoxes[1].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial3Clear()
    {
        yield return new WaitForSeconds(3f);
        msgBoxes[0].SetActive(true);
        Manager.manager.OpenMsgBox();
    }

    IEnumerator Tutorial4Clear()
    {
        yield return new WaitForSeconds(3f);
        msgBoxes[0].SetActive(true);
        Manager.manager.OpenMsgBox();
    }
}
