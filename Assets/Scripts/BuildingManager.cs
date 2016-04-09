using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public List<GameObject> Locations = new List<GameObject>();
    public List<GameObject> Doors = new List<GameObject>();
    public List<GameObject> Keys = new List<GameObject>();

    // 열쇠로 잠긴 문을 여는 메서드를 만들자.
    // 단, O(n^2)보다는 빠르게 만들어야겠지. (n개의 문의 ID와 n개의 열쇠의 target을 비교)
	void Start () {
	
	}
	
	void Update () {
	
	}
}
