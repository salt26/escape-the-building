using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    static public BuildingManager buildingManager;

    public List<GameObject> Locations = new List<GameObject>();
    public List<GameObject> Keys = new List<GameObject>();

    List<Location> AllLocations = new List<Location>();
    List<Location> HallLocations = new List<Location>();
    List<Location> RoomLocations = new List<Location>();

    //KeyInventory inventory;

    void Awake()
    {
        buildingManager = this;
    }

    void Start()
    {
        //inventory = GameObject.Find("Player").GetComponent<KeyInventory>();

        foreach (GameObject locationObject in Locations)
        {
            if (locationObject == null) continue;
            Location location = locationObject.GetComponent<Location>();
            if (location == null) continue;
            AllLocations.Add(location);
            if (location.GetLocationType() == 1) RoomLocations.Add(location);
            if (location.GetLocationType() == 2) HallLocations.Add(location);
        }

        // 열쇠를 랜덤으로 7개만 남기고 모두 지워버림
        while (Keys.Count > 7)
        {
            Keys.RemoveAt(Random.Range(0, Keys.Count));
        }

        RoomLocations.Remove(FindLocationByID(4105)); // 주인공 시작 지점 바로 앞에 열쇠가 생성되는 것 방지

        if (RoomLocations.Count < Keys.Count) // 비둘기집 원리
        {
            Debug.LogWarning("Cannot create keys!");
        }
        else
        {
            foreach (GameObject key in Keys)
            {
                Location selectedLocation = RoomLocations[Random.Range(0, RoomLocations.Count)];
                GameObject gameObject = (GameObject)Instantiate(key, selectedLocation.GetPosition() - new Vector3(0f, 1f, 0f), Quaternion.identity);
                gameObject.GetComponent<Key>().SetKeyID(selectedLocation.GetLocationID());
                RoomLocations.Remove(selectedLocation); // 비복원추출, RoomLocations는 일회용

            }
        }

        KeyText.ktxt.SetInitialKeyNumber(Keys.Count);
    }

    public Location FindLocationByID(int ID)
    {
        foreach (Location loc in AllLocations)
        {
            if (loc.GetLocationID() == ID) return loc;
        }
        return null;
    }

    public List<Location> GetAllLocations()
    {
        return AllLocations;
    }

    public List<Location> GetHallLocations()
    {
        return HallLocations;
    }
}
