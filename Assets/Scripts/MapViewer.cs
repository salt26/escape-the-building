using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapViewer : MonoBehaviour {

    public GameObject mapImages;
    public GameObject[] maps;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name != "TrainingRoom4" && SceneManager.GetActiveScene().name != "5.Building")
            {
                NoticeText.ntxt.NoticeMapInvalid();
                return;
            }
            else if (Move.move.GetTempZoneID() / 100 == 11)
            {
                SetAllMapInactive();
                maps[0].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 12)
            {
                SetAllMapInactive();
                maps[1].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 13)
            {
                SetAllMapInactive();
                maps[2].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 14)
            {
                SetAllMapInactive();
                maps[3].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 21)
            {
                SetAllMapInactive();
                maps[4].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 22)
            {
                SetAllMapInactive();
                maps[5].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 23)
            {
                SetAllMapInactive();
                maps[6].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 24)
            {
                SetAllMapInactive();
                maps[7].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 31)
            {
                SetAllMapInactive();
                maps[8].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 32)
            {
                SetAllMapInactive();
                maps[9].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 33)
            {
                SetAllMapInactive();
                maps[10].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 34)
            {
                SetAllMapInactive();
                maps[11].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 41)
            {
                SetAllMapInactive();
                maps[12].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 42 && SceneManager.GetActiveScene().name == "5.Building")
            {
                SetAllMapInactive();
                maps[13].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 43)
            {
                SetAllMapInactive();
                maps[14].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 51)
            {
                SetAllMapInactive();
                maps[15].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 52)
            {
                SetAllMapInactive();
                maps[16].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 53)
            {
                SetAllMapInactive();
                maps[17].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 54)
            {
                SetAllMapInactive();
                maps[18].SetActive(true);
                mapImages.SetActive(true);
                NoticeText.ntxt.NoticeMap(Move.move.GetTempZoneID());
            }
            else if (Move.move.GetTempZoneID() / 100 == 42 && SceneManager.GetActiveScene().name == "TrainingRoom4")
            {
                SetAllMapInactive();
                maps[0].SetActive(true);
                mapImages.SetActive(true);
            }
            else
            {
                NoticeText.ntxt.NoticeMapInvalid();
                return;
            }
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            mapImages.SetActive(false);
            SetAllMapInactive();
        }
	}


    void SetAllMapInactive()
    {
        foreach (GameObject thing in maps)
        {
            thing.SetActive(false);
        }
    }

}
