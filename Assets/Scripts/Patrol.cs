using UnityEngine;
using System.Collections;

/// <summary>
/// 추적자(캡슐)의 순찰 목적지를 계산하는 클래스
/// </summary>
public class Patrol : MonoBehaviour {

    public int patrolMode;              // 1이면 복도만(남자1), 2이면 복도와 잠기지 않은 방 전부를(남자2), 3이면 완전 랜덤으로(여자) 순찰함
    public GameObject playerLocation;
    public GameObject capsuleLocation;

    private Location tempLocation;      // 최근에 지난 지점(목적 지점으로 설정되지 않았더라도) -> Location.cs에서 설정해 줌
    private Location destLocation;      // 목적 지점
    private Location passedLocation;    // 이전에 마지막으로 설정되었던 목적 지점
    Transform target;
    Transform capsule;

	void Start () {
        tempLocation = null;
        destLocation = null;
        passedLocation = null;
        target = GetComponent<AutoMove>().target;
        capsule = GetComponent<Transform>();
	}

    void FixedUpdate()
    {
        /*
        RaycastHit rayHit;
        if(!Physics.Raycast(GetComponent<Transform>().position, target.position - GetComponent<Transform>().position, out rayHit)) {
            return;
        }
        */
        if (Input.GetKey(KeyCode.F1))
        {
            if(destLocation != null) Debug.Log("dest = " + destLocation.GetLocationID());
            if(passedLocation != null) Debug.Log("pass = " + passedLocation.GetLocationID());
            if(tempLocation != null) Debug.Log("temp = " + tempLocation.GetLocationID());
        }
        // 주인공을 목격하지 못했고 주인공의 발소리를 듣지 못했다면
        if (!GetComponent<AutoMove>().isRunning &&
            !(Move.move.isMoving && isSameFloor(target.position, capsule.position) &&
            ((isNearEnough(target.position, capsule.position, 10f) && Move.move.isRunning)) ||
            (isNearEnough(target.position, capsule.position, 7f) && !Move.move.isRunning)))     // (음량은 거리에 반비례)
        {
            if (tempLocation == destLocation)                                       // 목적 지점에 도달했는데
            {
                Location nextLocation;
                if (destLocation != null && destLocation.GetLocationType() != 3)    // 주인공을 쫓는 중이 아니었다면
                {
                    nextLocation = NextDestLocation(passedLocation, destLocation);  // 다음 목적지 재설정
                    passedLocation = destLocation;
                }
                else                                                                // 주인공을 쫓다가 놓쳤다면 (또는 목적 지점이 없으면)
                {
                    nextLocation = NextDestLocation(passedLocation, null);          // 마지막으로 지난 지점을 기준으로 순찰 경로에 합류
                }
                destLocation = nextLocation;
            }
            // 주인공을 쫓는 중이고 새로운 고정 지점을 지나면
            else if (destLocation != null && destLocation.GetLocationType() == 3 && tempLocation != destLocation && tempLocation != passedLocation)
            {
                // 그 지점을 마지막으로 지난 지점으로 설정
                passedLocation = tempLocation;
            }
        }
        else // 주인공을 감지한 경우
        {
            // 기존 목적 지점을 버리고 주인공을 우선적으로 쫓아감
            playerLocation.GetComponent<Transform>().position = target.position + new Vector3(0f, 0.2f, 0f);
            destLocation = playerLocation.GetComponent<Location>();
        }
        /* 한 자리에 멈춰서 4초(?) 이상 움직이지 않는 경우 주인공 무시하고 무조건 경로를 재탐색하도록 하기 */
        /* 체력 시스템 도입하면 탈진 시 경로 재설정하도록 하기 */
    }

    // 같은 층에서 A, B 사이의 거리가 distance보다 가까우면 true 반환
    bool isNearEnough(Vector3 A, Vector3 B, float distance)
    {
        if (Mathf.Pow(A.x - B.x, 2f) + Mathf.Pow(A.z - B.z, 2f) < Mathf.Pow(distance, 2f)) return true;
        else return false;
    }

    // A, B가 같은 층이면 true 반환
    bool isSameFloor(Vector3 A, Vector3 B)
    {
        if (Mathf.Abs(A.y - B.y) <= 1.5f) return true;
        else return false;
    }

    // patrolMode가 1 또는 2일 때 다음 목적 지점을 계산하여 반환함
    Location NextDestLocation(Location passed, Location arrived)
    {
        int index, nextIndex;
        if (patrolMode == 0) Debug.Log("Capsule's patrolMode is 0");
        if (patrolMode == 1) // 복도만 돌아다님
        {
            if (arrived == null) // 도착 지점이 없는 경우
            {
                if (passed != null && passed.GetLocationType() == 2) // 마지막으로 지난 복도 지점이 있다면
                {
                    // 마지막으로 지난 지점과 인접한 랜덤 복도 지점을 반환
                    nextIndex = Random.Range(0, passed.GetAdjacentHallLocations().Count);
                    return passed.GetAdjacentHallLocations()[nextIndex];
                }
                else // 마지막으로 지난 지점도 없다면 (또는 마지막으로 지난 지점이 복도 지점이 아니라면)
                {
                    // 모든 복도 지점 중 랜덤 지점을 반환
                    nextIndex = Random.Range(0, BuildingManager.buildingManager.GetHallLocations().Count);
                    return BuildingManager.buildingManager.GetHallLocations()[nextIndex];
                }
            }
            else if (passed != null && (index = arrived.GetAdjacentHallLocations().IndexOf(passed)) != -1) // 인접한 복도 지점에서 온 경우
            {
                // 마지막으로 지난 지점을 제외하고 도착 지점과 인접한 랜덤 복도 지점을 반환
                nextIndex = Random.Range(1, arrived.GetAdjacentHallLocations().Count);
                if (nextIndex == index) nextIndex = 0;
                return arrived.GetAdjacentHallLocations()[nextIndex];
            }
            else // 도착 지점에 오기 전 마지막으로 지난 지점이 현재 도착한 지점과 인접하지 않은 경우 또는 마지막으로 지난 지점이 없는 경우
            {
                // 도착 지점과 인접한 랜덤 복도 지점을 반환
                nextIndex = Random.Range(0, arrived.GetAdjacentHallLocations().Count);
                return arrived.GetAdjacentHallLocations()[nextIndex];
            }
        }
        else if (patrolMode == 2) // 복도와 방 모두 돌아다님
        {
            if (arrived == null) // 도착 지점이 없는 경우
            {
                if (passed != null) // 마지막으로 지난 지점이 있다면
                {
                    // 마지막으로 지난 지점과 인접한 랜덤 지점을 반환
                    nextIndex = Random.Range(0, passed.GetAdjacentAllLocations().Count);
                    return passed.GetAdjacentAllLocations()[nextIndex];
                }
                else // 마지막으로 지난 지점도 없다면
                {
                    // 모든 지점 중 랜덤 지점을 반환
                    nextIndex = Random.Range(0, BuildingManager.buildingManager.GetAllLocations().Count);
                    return BuildingManager.buildingManager.GetAllLocations()[nextIndex];
                }
            }
            else if (passed != null && (index = arrived.GetAdjacentAllLocations().IndexOf(passed)) != -1) // 인접한 지점에서 온 경우
            {
                // 마지막으로 지난 지점을 제외하고 도착 지점과 인접한 랜덤 지점을 반환
                nextIndex = Random.Range(1, arrived.GetAdjacentAllLocations().Count);
                if (nextIndex == index) nextIndex = 0;
                return arrived.GetAdjacentAllLocations()[nextIndex];
            }
            else // 도착 지점에 오기 전 마지막으로 지난 지점이 현재 도착한 지점과 인접하지 않은 경우 또는 마지막으로 지난 지점이 없는 경우
            {
                // 도착 지점과 인접한 랜덤 지점을 반환
                nextIndex = Random.Range(0, arrived.GetAdjacentAllLocations().Count);
                return arrived.GetAdjacentAllLocations()[nextIndex];
            }
        }
        else
        {
            return NextDestLocation();
        }
    }

    // patrolMode가 3일 때 다음 목적 지점을 계산하여 반환함
    public Location NextDestLocation()
    {
        int nextIndex = Random.Range(0, BuildingManager.buildingManager.GetAllLocations().Count);
        return BuildingManager.buildingManager.GetAllLocations()[nextIndex];
    }

    public Vector3 GetDestPosition()
    {
        if (destLocation == null) return Vector3.zero;
        return destLocation.GetPosition();
    }

    public Location GetTempLocation()
    {
        return tempLocation;
    }

    public void SetTempLocation(Location temp)
    {
        tempLocation = temp;
    }
}
