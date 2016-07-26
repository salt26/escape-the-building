#define NEW_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif

public class Location : MonoBehaviour {

    [SerializeField] int locationID; // 지점의 고유번호
    [SerializeField] int locationType; // 지점의 종류가 방이면 1, 복도이면 2, 주인공이면 3, 추적자이면 4

    public List<GameObject> adjacentAllLocations = new List<GameObject>(); // 인접한 모든 지점의 GameObject
    public List<GameObject> adjacentHallLocations = new List<GameObject>(); // 인접한 복도 지점의 GameObject(종류가 방이면 해당 없음)

    private List<Location> adjacentAll = new List<Location>(); // 인접한 모든 지점
    private List<Location> adjacentHall = new List<Location>(); // 인접한 복도 지점

    Transform location;

    void Awake()
    {
        location = GetComponent<Transform>();
    }
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        foreach (GameObject locationObject in adjacentAllLocations)
        {
            Location loc = locationObject.GetComponent<Location>();
            if (loc == null) continue;
            adjacentAll.Add(loc);
        }
        foreach (GameObject locationObject in adjacentHallLocations)
        {
            Location loc = locationObject.GetComponent<Location>();
            if (loc == null) continue;
            adjacentHall.Add(loc);
        }
    }

    public int GetLocationID()
    {
        return locationID;
    }

    public int GetLocationType()
    {
        return locationType;
    }

    public Vector3 GetPosition()
    {
        return location.position;
    }

    public List<Location> GetAdjacentAllLocations()
    {
        return adjacentAll;
    }

    public List<Location> GetAdjacentHallLocations()
    {
        return adjacentHall;
    }

    void OnTriggerEnter(Collider other)
    {
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "3.Modeling") {
            return;
        }
#endif
        if (this.GetLocationType() <= 3 && other.name == "Capsule" && other.gameObject.GetComponent<Patrol>().GetTempLocation() != this)
        {
            Debug.Log(this.GetLocationID());
            other.gameObject.GetComponent<Patrol>().SetTempLocation(this);
        }
    }
}
