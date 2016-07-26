using UnityEngine;
using System.Collections;

/// <summary>
/// 추적자(캡슐)의 순찰 목적지를 계산하는 클래스
/// </summary>
public class Patrol : MonoBehaviour {

    public int patrolMode;          // 1이면 복도만(남자1), 2이면 복도와 잠기지 않은 방 전부를(남자2), 3이면 완전 랜덤으로(여자) 순찰함

    private Location tempLocation;      // 최근에 지난 지점(목적 지점으로 설정되지 않았더라도) -> Location.cs에서 설정해 줌
    private Location destLocation;      // 목적 지점
    private Location passedLocation;    // 이전에 마지막으로 설정되었던 목적 지점

	void Start () {
        tempLocation = null;
        destLocation = null;
        passedLocation = null;
	}

    void FixedUpdate()
    {
        if (tempLocation == destLocation) // 목적 지점에 도달하면(그리고 주인공을 쫓는 상태가 아니라면)
        {
            Location nextLocation = NextDestLocation(passedLocation, destLocation); // 다음 목적지 재설정
            passedLocation = destLocation;
            destLocation = nextLocation;
        }
    }

    // patrolMode가 1 또는 2일 때 다음 목적 지점을 계산하여 반환함
    private Location NextDestLocation(Location passed, Location arrived)
    {
        int index, nextIndex;
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
