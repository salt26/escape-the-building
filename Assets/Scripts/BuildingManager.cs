using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    static public BuildingManager buildingManager;

    public List<GameObject> Locations = new List<GameObject>();
    public List<GameObject> Doors = new List<GameObject>();
    public List<GameObject> Keys = new List<GameObject>();

    List<Location> AllLocations = new List<Location>();
    List<Location> RoomLocations = new List<Location>();
    List<Door> AllDoors = new List<Door>();

    KeyInventory inventory;

    void Awake()
    {
        buildingManager = this;
    }

    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<KeyInventory>();

        foreach (GameObject locationObject in Locations)
        {
            Location location = locationObject.GetComponent<Location>();
            if (location == null) continue;
            AllLocations.Add(location);
            if (location.GetLocationType() == 1) RoomLocations.Add(location);
        }

        if (RoomLocations.Count < Keys.Count) // 비둘기집 원리
        {
            Debug.LogWarning("Cannot create keys!");
        }
        else
        {
            foreach (GameObject key in Keys)
            {
                Location selectedLocation = RoomLocations[Random.Range(0, RoomLocations.Count)];
                GameObject gameObject = (GameObject)Instantiate(key, selectedLocation.GetPosition(), Quaternion.identity);
                gameObject.GetComponent<Key>().SetKeyID(selectedLocation.GetLocationID());
                RoomLocations.Remove(selectedLocation); // 비복원추출, RoomLocations는 일회용

            }
        }

        foreach (GameObject doorObject in Doors)
        {
            Door door = doorObject.GetComponent<Door>();
            if (door == null) continue;
            AllDoors.Add(door);
        }

        KeyText.ktxt.SetInitialKeyNumber(Keys.Count);
    }

    public int NumberOfUsedKey()
    {
        int num = 0;
        foreach (KeyInInventory key in inventory.keyInventory)
        {
            if (key.GetIsUsed()) num++;
        }
        return num;
    }

    public int NumberOfNotUsedKey()
    {
        int num = 0;
        foreach (KeyInInventory key in inventory.keyInventory)
        {
            if (!key.GetIsUsed()) num++;
        }
        return num;
    }
	
}
