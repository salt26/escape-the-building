﻿#define NEW_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif

public class State : MonoBehaviour {

    public GameObject stateImage;

    List<GameObject> images = new List<GameObject>();

	void Start () {
        for (int i = 0; i < Manager.manager.speakers.Length; i++)
        {
            GameObject newImage;
#if NEW_VERSION
            if(SceneManager.GetActiveScene().name=="1.Terrain and Audio")
                newImage = Instantiate(stateImage, new Vector3(-240f + 40f * (float)i, -15f), Quaternion.identity) as GameObject;
            else
#endif
                newImage = Instantiate(stateImage, new Vector3(-340f + 40f * (float)i, -15f), Quaternion.identity) as GameObject;
            newImage.transform.SetParent(GetComponent<Transform>(), false);
            newImage.GetComponent<StateImage>().SetSpeaker(i);
            images.Add(newImage);
        }

	}

    void FixedUpdate()
    {
        for (int i = 0; i < Manager.manager.speakers.Length; i++)
        {
            if (Manager.manager.IsSpeakerTouched(i))
            {
                images[i].GetComponent<StateImage>().Clear();
            }
        }
    }
}
