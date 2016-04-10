using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Location : MonoBehaviour {

    [SerializeField] int locationID; // 지점의 고유번호
    [SerializeField] int locationType; // 지점의 종류가 방이면 1, 복도이면 2, 주인공이면 3

    public List<GameObject> adjacentAllLocations = new List<GameObject>(); // 인접한 모든 지점
    public List<GameObject> adjacentHallLocations = new List<GameObject>(); // 인접한 복도 지점(종류가 방이면 해당 없음)

    Transform location;

    void Start()
    {
        location = GetComponent<Transform>();
        GetComponent<MeshRenderer>().enabled = false;
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
}
